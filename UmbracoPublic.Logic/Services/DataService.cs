﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using LinqIt.Search;
using LinqIt.Utils.Caching;
using LinqIt.Utils.Extensions;
using UmbracoPublic.Interfaces;
using UmbracoPublic.Logic.BackgroundWork;
using UmbracoPublic.Logic.Entities;

namespace UmbracoPublic.Logic.Services
{
    public class DataService
    {
        private CookieState? _cookieState;

        public static DataService Instance
        {
            get
            {
                return Cache.Get(typeof(DataService).FullName, CacheScope.Request, () => new DataService());
            }
        }

        public IEnumerable<MenuItem> GetTopMenuItems()
        {
            var result = new List<MenuItem>();
            Page page = CmsService.Instance.GetHomeItem();
            foreach (var child in page.GetChildren<Page>().Where(c => !c.EntityName.StartsWith("__") && !c.GetValue<bool>("hideFromMenu")))
            {
                var item = GetMenuItem(child);
                foreach (var subChild in child.GetChildren<Page>().Where(sc => !sc.EntityName.StartsWith("__") && !sc.GetValue<bool>("hideFromMenu")))
                {
                    var childItem = GetMenuItem(subChild);
                    item.AddChild(childItem);
                }
                result.Add(item);
            }
            return result;
        }

        public IEnumerable<MenuItem> GetSubMenuItems()
        {
            var homeParts = CmsService.Instance.GetHomeItem().Path.Split('/');
            var page = CmsService.Instance.GetItem<Page>();
            var pageParts = page.Path.Split('/');
            if (pageParts.Length - homeParts.Length < 1)
                return new MenuItem[0];

            return page.GetChildren<Page>().Where(c => !c.EntityName.StartsWith("__")).Select(GetMenuItem).ToList();
        }

        private static MenuItem GetMenuItem(Page page)
        {
            if (page.EntityName.StartsWith("__"))
                return null;

            var item = new MenuItem();
            item.DisplayName = page.EntityName;
            item.Url = page.Url;
            return item;
        }

        public IEnumerable<MenuItem> GetSideMenuItems()
        {
            var menuIds = CmsService.Instance.GetSelectedMenuIds();
            var home = CmsService.Instance.GetHomeItem();
            var parent = CmsService.Instance.GetItem<Page>();
            if (parent.Id == home.Id)
                return new MenuItem[0];

            var tmp = parent.ParentPage;
            while (tmp.Id != home.Id)
            {
                parent = tmp;
                tmp = tmp.ParentPage;
            }

            var topMenuItem = GetMenuItemRecursive(parent, menuIds);
            return new []{ topMenuItem};
        }

        private static MenuItem GetMenuItemRecursive(Page page, ICollection<Id> menuIds)
        {
            var result = GetMenuItem(page);
            if (result == null)
                return null;
            
            result.Selected = page.Id == menuIds.Last();
            var children = page.GetChildren<Page>().Where(c => !c.GetValue<bool>("hideFromMenu")).ToArray();
            if (children.Any())
            {
                if (menuIds.Contains(page.Id))
                {
                    result.Expanded = true;
                    foreach (var child in children)
                    {
                        var childItem = GetMenuItemRecursive(child, menuIds);
                        if (childItem != null)
                            result.AddChild(childItem);
                    }
                }
                else
                    result.Collapsed = true;
            }
            return result;
        }

        public SearchResult PerformSearch(SearchFilter filter)
        {
            using (var service = new SearchService("site"))
            {
                var site = CmsService.Instance.SitePath.Split('/').Last();

                var query = new QueryList();
                query.SubQueries.Add(new TermQuery("site", site));

                if (!string.IsNullOrEmpty(filter.TemplateName))
                    query.SubQueries.Add(new TermQuery("template", filter.TemplateName));
                if (!string.IsNullOrEmpty(filter.Query))
                {
                    foreach (var term in filter.Query.ToLower().Split(' '))
                        query.SubQueries.Add(new WildCardQuery("text", term));
                }
                if (filter.From.HasValue || filter.To.HasValue)
                    query.SubQueries.Add(new DateRangeQuery("date", filter.From, filter.To));
                if (filter.CategorizationIds != null && filter.CategorizationIds.Any())
                {
                    if (filter.CategorizationIds.Length == 1)
                        query.SubQueries.Add(new WildCardQuery("categorizations", filter.CategorizationIds[0].ToString()));
                    else
                    {
                        var categorizationFolder = CategorizationFolder.Get();
                        var types = categorizationFolder.GetTypes(filter.CategorizationIds);
                        var typeQueries = new List<Query>();
                        foreach (var typeIds in types.Select(type => filter.CategorizationIds.Where(type.HasItem).ToArray()))
                        {
                            if (typeIds.Length == 1)
                                typeQueries.Add(new WildCardQuery("categorizations", typeIds[0].ToString()));
                            else
                                typeQueries.Add(BooleanQuery.Or(typeIds.Select(id => new WildCardQuery("categorizations", id.ToString())).ToArray()));
                        }
                        query.SubQueries.Add(BooleanQuery.And(typeQueries.ToArray()));
                    }
                }
                    
                return service.Search(query, 0, int.MaxValue);
            }
        }

        public Dictionary<Id, string> GetCategorizations()
        {
            var currentItem = CmsService.Instance.GetItem<Entity>();
            var folderPath = CmsService.Instance.GetSystemPath("CategorizationFolder", currentItem.Path);
            var folder = CmsService.Instance.GetItem<Entity>(folderPath);
            return folder.GetChildren<Entity>().ToDictionary(item => item.Id, item => item.EntityName);
        }

        private static IMailProvider GetMailProvider()
        {
            var mailConfiguration = CmsService.Instance.GetConfigurationItem<MailConfiguration>("Mail");
            return mailConfiguration.MailProvider;
        }

        internal void SubscribeToNewsletter(string emailAddress, string newsListId)
        {
            var mailProvider = GetMailProvider();
            mailProvider.SubscribeToList(newsListId, emailAddress, new NameValueCollection());
        }

        public CookieState GetCookieState()
        {
            if (!_cookieState.HasValue)
            {
                var cookie = HttpContext.Current.Request.Cookies.Get(HttpContext.Current.Request.Url.Host + "_CookieState");
                if (cookie == null)
                    _cookieState = CookieState.NotAccepted;
                else
                {
                    _cookieState = cookie.Value == "Accepted" ? CookieState.Accepted : CookieState.NotAccepted;
                }
            }
            return _cookieState.Value;
        }

        internal void AcceptCookies()
        {
            var cookie = new HttpCookie(HttpContext.Current.Request.Url.Host + "_CookieState", "Accepted");
            cookie.Expires = DateTime.Today.AddYears(1);
            HttpContext.Current.Response.Cookies.Add(cookie);
            _cookieState = CookieState.Accepted;
        }

        internal void DeleteCookies()
        {
            var protectedCookies = new[] { "ASP.NET_SessionId", "UMB_UPDCHK", "UMB_UCONTEXT" };
            var cookies = HttpContext.Current.Request.Cookies.AllKeys.Except(protectedCookies).ToArray();
            foreach (var cookie in cookies)
                DeleteCookie(cookie);
            _cookieState = CookieState.NotAccepted;
        }

        private static void DeleteCookie(string name)
        {
            var cookie = new HttpCookie(name);
            cookie.Expires = DateTime.Today.AddDays(-1);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    
        public void RebuildSearchIndex()
        {
            using (var service = new CrawlService("site"))
            {
                service.ClearDatabase();
            }
            Queue<Page> pages = new Queue<Page>();
            using (CmsContext.Published)
            {
                var siteRoots = CmsService.Instance.SelectItems<SiteRoot>("/Content/*/*{SiteRoot}");
                pages.EnqueueRange(siteRoots.SelectMany(r => r.GetChildren<Page>()));
                while (pages.Count > 0)
                {
                    var page = pages.Dequeue();
                    if (page.Template.Path.StartsWith("/WebPage"))
                    {
                        var site = CmsService.Instance.GetSitePath(page.Path).Split('/').Last();
                        var thumbnail = page.GetValue<Image>("thumbnail");
                        SearchBackgroundCrawler.QueueDocumentAdd(site, page, thumbnail.Exists ? thumbnail.Url : string.Empty);
                    }
                    pages.EnqueueRange(page.GetChildren<Page>());
                }
            }
        }
    }
}
