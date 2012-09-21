using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using LinqIt.Utils.Extensions;

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
                return null;
            var page = CmsService.Instance.GetItem<Page>(path);
            if (page == null)
                return null;
            return page.Url;
        }

        internal static string GetMainNewsListUrl()
        {
            return GetSystemUrl(SystemKey.MainNewsPage);
        }

        internal static string GetCookieAcceptancePageUrl()
        {
            return GetSystemUrl(SystemKey.CookieAcceptancePage);
        }

        public static string MonthArray { get { return "jan|feb|mar|apr|maj|jun|jul|aug|sep|okt|nov|dec"; } }

        internal static void GetNewsListDates(out DateTime? from, out DateTime? to)
        {
            var localPath = HttpContext.Current.Request.Url.LocalPath;
            var yearRegex = new Regex(@"/(?<year>\d{4})(?:\.aspx)$", RegexOptions.IgnoreCase);
            var yearMatch = yearRegex.Match(localPath);
            if (yearMatch.Success)
            {
                from = new DateTime(Convert.ToInt32(yearMatch.Groups["year"].Value), 1, 1);
                to = from.Value.AddYears(1).AddDays(-1);
                return;
            }

            var monthRegex = new Regex(@"/(?<year>\d{4})/(?<month>" + MonthArray + @")(?:\.aspx)?$");
            var monthMatch = monthRegex.Match(localPath);
            if (monthMatch.Success)
            {
                from = new DateTime(Convert.ToInt32(monthMatch.Groups["year"].Value), MonthArray.Split('|').ToList().IndexOf(monthMatch.Groups["month"].Value.ToLower()) + 1, 1);
                to = from.Value.AddMonths(1).AddDays(-1);
                return;
            }
            from = null;
            to = null;
        }
    }
}
