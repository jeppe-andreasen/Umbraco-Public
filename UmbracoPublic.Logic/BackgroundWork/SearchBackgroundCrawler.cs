using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using LinqIt.Search;
using LinqIt.Utils;
using LinqIt.Utils.Extensions;
using UmbracoPublic.Logic.Entities;
using umbraco.cms.businesslogic.web;

namespace UmbracoPublic.Logic.BackgroundWork
{
    public static class SearchBackgroundCrawler
    {
        private static readonly ConcurrentQueue<SearchTask> _queue = new ConcurrentQueue<SearchTask>();
        private static Thread _backgroundThread;

        internal static void QueueDocumentAdd(string site, Page page, string thumbnail)
        {
            Logging.Log(LogType.Info, "Adding SearchTask:AddDocument, " + page.Path);
            _queue.Enqueue(new AddDocumentTask(site, page, thumbnail));
            RunThread();
        }

        internal static void QueueDocumentDelete(Page page)
        {
            Logging.Log(LogType.Info, "Adding SearchTask:RemoveDocument, " + page.Path);
            _queue.Enqueue(new RemoveDocumentTask(page));
            RunThread();
        }

        private static void RunThread()
        {
            if (_backgroundThread != null)
                return;

            _backgroundThread = new Thread(Execute);
            _backgroundThread.Start();
        }

        private static void Execute()
        {
            SearchTask task;
            _queue.TryDequeue(out task);

            if (task == null)
                return;
            using (var service = new CrawlService("site"))
            {
                Logging.Log(LogType.Info, "CrawlService opened");
                while (task != null)
                {
                    task.Process(service);
                    _queue.TryDequeue(out task);
                }
                Logging.Log(LogType.Info, "CrawlService closed");
            }
            _backgroundThread = null;
        }

        public abstract class SearchTask
        {
            protected SearchTask(Page page)
            {
                Name = page.EntityName;
                Url = page.Url;
            }

            public string Name { get; private set; }

            public string Url { get; private set; }

            public abstract void Process(CrawlService service);
        }

        public class AddDocumentTask : SearchTask
        {
            public AddDocumentTask(string site, Page page, string thumbnail) : base(page)
            {
                Site = site;
                NoIndex = page.GetValue<bool>("noIndex");
                if (NoIndex) 
                    return;

                Title = page["headline"];
                if (string.IsNullOrEmpty(Title))
                    Title = page.EntityName;

                TemplateName = page.Template.Name;

                if (TemplateName == "NewsPage")
                {
                    var newsPage = page.CastAs<NewsPage>();
                    Date = newsPage.Date;
                    Categorizations = (newsPage["categorizations"]?? string.Empty).Split(',','|',';');
                }

                Summary = page["metaDescription"];
                if (string.IsNullOrEmpty(Summary))
                    Summary = page["intro"];
                if (string.IsNullOrEmpty(Summary))
                {
                    var html = page.GetValue<Html>("body");
                    if (!html.IsEmpty)
                        Summary = html.GetExtract(150, true).ToString();
                }

                Thumbnail = thumbnail;
            }

            public string Site { get; private set; }

            public bool NoIndex { get; private set; }

            public string Title { get; private set; }

            public DateTime? Date { get; private set; }

            public string TemplateName { get; private set; }

            public string[] Categorizations { get; private set; }

            public string Summary { get; private set; }

            public string Thumbnail { get; private set; }


            public override void Process(CrawlService service)
            {
                try
                {
                    if (NoIndex)
                    {
                        service.RemoveRecord(Url);
                    }
                    else
                    {
                        var record = service.GetRecordFromUrl(Url, "text", Title);
                        if (record != null)
                        {
                            record.SetString("site", Site);
                            record.SetString("title", Title);
                            record.SetString("template", TemplateName);
                            record.SetString("categorizations", Categorizations != null && Categorizations.Any() ? Categorizations.ToSeparatedString(",").ToLower() : string.Empty);
                            record.SetString("summary", Summary);
                            record.SetDate("date", Date);
                            record.SetString("thumbnail", Thumbnail);
                            service.AddRecord(record);
                        }
                    }
                }
                catch(Exception exc)
                {
                    var record = service.NewRecord(Url);
                    record.SetString("error", exc.ToString());
                    service.AddRecord(record);
                }
            }
        }

        public class RemoveDocumentTask : SearchTask
        {
            public RemoveDocumentTask(Page page) : base(page)
            {
            }

            public override void Process(CrawlService service)
            {
                service.RemoveRecord(Url);
            }
        }
    }
}
