using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
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

        public static string GetSystemUrl(SystemKey key)
        {
            var path = CmsService.Instance.GetSystemPath(key.ToString());
            if (string.IsNullOrEmpty(path))
                throw new ConfigurationErrorsException("System key not specified: " + key);
            return CmsService.Instance.GetItem<Page>(path).Url;
        }

        internal static string GetMainNewsListUrl()
        {
            return GetSystemUrl(SystemKey.MainNewsPage);
        }

        internal static string GetCookieAcceptancePageUrl()
        {
            return GetSystemUrl(SystemKey.CookieAcceptancePage);
        }
    }
}
