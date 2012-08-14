using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using LinqIt.Cms.Data;
using LinqIt.Utils.Extensions;
using UmbracoPublic.Logic.Services;

namespace UmbracoPublic.Logic.Entities
{
    public class SearchFilter
    {
        public SearchFilter(NameValueCollection queryString)
        {
            Query = queryString["query"];
            if (!string.IsNullOrEmpty(queryString["from"]))
                From = DateTime.ParseExact(queryString["from"], "dd-MM-yyyy", CultureInfo.InvariantCulture);
            if (!string.IsNullOrEmpty(queryString["to"]))
                To = DateTime.ParseExact(queryString["to"], "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var subjectName = queryString["subject"];
            if (!string.IsNullOrEmpty(subjectName))
            {
                var subjects = DataService.Instance.GetSubjects();
                foreach (var pair in subjects.Where(pair => string.Compare(pair.Value, subjectName, true) == 0))
                {
                    SubjectIds = new[] { pair.Key };
                    break;
                }
            }
        }

        public static SearchFilter FromUrl()
        {
            return new SearchFilter(HttpContext.Current.Request.QueryString);
        }

        public string Query { get; set; }

        public string TemplateName { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public Id[] SubjectIds { get; set; }
    }
}
