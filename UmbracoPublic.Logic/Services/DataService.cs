using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using LinqIt.Search;
using LinqIt.Utils.Caching;
using UmbracoPublic.Logic.Entities;

namespace UmbracoPublic.Logic.Services
{
    public class DataService
    {
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
            foreach (var child in page.GetChildren<Page>().Where(c => !c.EntityName.StartsWith("__")))
            {
                var item = GetMenuItem(child);
                foreach (var subChild in child.GetChildren<Page>().Where(sc => !sc.EntityName.StartsWith("__")))
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
            if (pageParts.Length - homeParts.Length < 2)
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

            var result = new List<MenuItem>();
            AddChildren(result, menuIds, parent);
            return result;
        }

        private static void AddChildren(ICollection<MenuItem> collection, ICollection<Id> menuIds, Entity parent)
        {
            foreach (var child in parent.GetChildren<Page>())
            {
                var menuItem = GetMenuItem(child);
                if (menuItem == null)
                    continue;

                collection.Add(menuItem);

                if (menuIds.Contains(child.Id))
                {
                    menuItem.Active = true;
                    var childCollection = new List<MenuItem>();
                    AddChildren(childCollection, menuIds, child);
                    foreach (var childMenuItem in childCollection)
                        menuItem.AddChild(childMenuItem);
                }
            }
        }

        public SearchResult PerformSearch(SearchFilter filter)
        {
            using (var service = new SearchService("site"))
            {
                var query = new QueryList();
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
    }
}
