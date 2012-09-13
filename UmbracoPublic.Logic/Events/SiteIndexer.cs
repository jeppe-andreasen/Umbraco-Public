using System;
using System.Linq;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using LinqIt.Search;
using umbraco;
using umbraco.cms.businesslogic;
using UmbracoPublic.Logic.BackgroundWork;
using umbraco.cms.businesslogic.web;
using UmbracoPublic.Logic.Entities;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.Logic.Events
{
    public class SiteIndexer : umbraco.BusinessLogic.ApplicationBase
    {
        public SiteIndexer()
        {
            Document.BeforeUnPublish += OnDocumentUnpublished;   
            Document.AfterPublish += OnDocumentPublished;
            Document.AfterDelete += OnDocumentDeleted;
            Document.AfterSave += OnDocumentSaved;
            Document.AfterMove += OnDocumentMoved;
        }

        private static void OnDocumentUnpublished(Document sender, UnPublishEventArgs e)
        {
            var page = CmsService.Instance.GetItem<Page>(new Id(sender.Id));
            if (page == null)
                return;
            if (!page.Template.Path.StartsWith("/WebPage"))
                return;
            SearchBackgroundCrawler.QueueDocumentDelete(page);
        }

        protected static void OnDocumentDeleted(Document sender, umbraco.cms.businesslogic.DeleteEventArgs e)
        {
            var page = CmsService.Instance.GetItem<Page>(new Id(sender.Id));
            if (!page.Template.Path.StartsWith("/WebPage"))
                return;
            SearchBackgroundCrawler.QueueDocumentDelete(page);
        }

        protected static void OnDocumentPublished(Document sender, umbraco.cms.businesslogic.PublishEventArgs e)
        {
            global::umbraco.library.UpdateDocumentCache(sender.Id);
            var page = CmsService.Instance.GetItem<Page>(new Id(sender.Id));
            if (!page.Template.Path.StartsWith("/WebPage"))
                return;

            var site = CmsService.Instance.GetSitePath(page.Path).Split('/').Last();

            var thumbnail = page.GetValue<Image>("thumbnail");
            SearchBackgroundCrawler.QueueDocumentAdd(site, page, thumbnail.Exists? thumbnail.Url : string.Empty);
        }

        private static void OnDocumentSaved(Document sender, SaveEventArgs e)
        {
            if (sender.ContentType.Alias == "NewsPage")
            {
                using (CmsContext.Editing)
                {
                    var newspage = CmsService.Instance.GetItem<UmbracoPublic.Logic.Entities.NewsPage>(new Id(sender.Id));
                    if (newspage.Date.HasValue)
                    {
                        var newsArchivePart = CmsService.Instance.GetSystemPath("NewsArchivePage");
                        var yearPart = newspage.Date.Value.Year.ToString();
                        var monthPart = Urls.MonthArray.Split('|')[newspage.Date.Value.Month - 1];
                        var truePath = Paths.Combine(newsArchivePart, yearPart, monthPart, sender.Text);
                        if (truePath != newspage.Path)
                        {
                            var yearPage = CmsService.Instance.GetItem<NewsListPage>(Paths.Combine(newsArchivePart, yearPart));
                            if (yearPage == null)
                            {
                                var archivePage = CmsService.Instance.GetItem<NewsListPage>(newsArchivePart);
                                yearPage = CmsService.Instance.CreateEntity<NewsListPage>(yearPart, archivePage);
                            }
                            var monthPage =
                                CmsService.Instance.GetItem<NewsListPage>(Paths.Combine(newsArchivePart, yearPart,
                                                                                        monthPart));
                            if (monthPage == null)
                                monthPage = CmsService.Instance.CreateEntity<NewsListPage>(monthPart, yearPage);
                            sender.Move(monthPage.Id.IntValue);

                            var page = umbraco.BasePages.BasePage.Current;
                            page.ClientScript.RegisterStartupScript(page.GetType(), "refreshItem", "top.window.location.href = '/umbraco/umbraco.aspx?app=content&rightAction=editContent&id=" + sender.Id + "#content';", true);
                        }
                    }
                }
            }
        }

        private static void OnDocumentMoved(object sender, MoveEventArgs e)
        {
            if (!(sender is Document))
                return;

            var document = (Document)sender;
            global::umbraco.library.UpdateDocumentCache(document.Id);
            if (document.Published)
            {
                document.UnPublish();
                library.UnPublishSingleNode(document.Id);
                document.PublishWithResult(document.User);
            }
        }
    }
}

