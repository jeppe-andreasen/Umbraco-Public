using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using LinqIt.Utils.Extensions;
using LinqIt.Utils.Web;

namespace UmbracoPublic.Logic.Utilities
{
    public static class Urls
    {
        public static string ReplaceUrlParameter(QueryStringKey key, string value)
        {
            var result = CmsService.Instance.CurrentUrl != null ? CmsService.Instance.CurrentUrl.LocalPath : string.Empty;

            var collection = new NameValueCollection(HttpContext.Current.Request.QueryString);
            if (value == null)
                collection.Remove(key.ToString());
            else
                collection[key.ToString()] = value;
            if (collection.Count > 0)
                result += "?" + collection.ToUrlParameterList();

            return result;
        }

        public static string ReplaceUrlParameters(Dictionary<QueryStringKey, string> replacements)
        {
            var result = CmsService.Instance.CurrentUrl != null ? CmsService.Instance.CurrentUrl.LocalPath : string.Empty;
            
            var collection = new NameValueCollection(HttpContext.Current.Request.QueryString);
            foreach (var key in replacements.Keys)
                collection[key.ToString()] = replacements[key];
            if (collection.Count > 0)
                result += "?" + collection.ToUrlParameterList();
            
            return result;
        }

        internal static string GetMainNewsListUrl()
        {
            var path = CmsService.Instance.GetSystemPath("MainNewsPage");
            return CmsService.Instance.GetItem<Page>(path).Url;
        }
    }
}
