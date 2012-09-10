using LinqIt.Cms;
using LinqIt.Cms.Data;
using LinqIt.Search;
using UmbracoPublic.Logic.BackgroundWork;
using umbraco.cms.businesslogic.web;

namespace UmbracoPublic.Logic.Events
{
    public class SiteIndexer : umbraco.BusinessLogic.ApplicationBase
    {
        public SiteIndexer()
        {
            Document.AfterPublish += OnDocumentPublished;
            Document.AfterDelete += OnDocumentDeleted;
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

            var thumbnail = page.GetValue<Image>("thumbnail");
            SearchBackgroundCrawler.QueueDocumentAdd(page, thumbnail.Exists? thumbnail.Url : string.Empty);
        }
    }
}

