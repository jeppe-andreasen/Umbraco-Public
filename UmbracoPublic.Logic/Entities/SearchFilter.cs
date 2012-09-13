using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using LinqIt.Ajax.Parsing;
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
            this.CategorizationIds = new IdList(queryString["categorizations"]).ToArray();
        }

        private SearchFilter(JSONObject json)
        {
            if (json.HasKey("q"))
                Query = (string) json["q"];
            if (json.HasKey("t"))
                TemplateName = (string) json["t"];
            if (json.HasKey("df"))
                From = (DateTime) json["df"];
            if (json.HasKey("dt"))
                To = (DateTime) json["dt"];
            CategorizationIds = json.HasKey("cids") ? ((JSONArray) json["cids"]).Values.Select(v => new Id(v)).ToArray() : new Id[0];
        }

        public static SearchFilter FromUrl()
        {
            return new SearchFilter(HttpContext.Current.Request.QueryString);
        }

        internal static SearchFilter FromString(string filter)
        {
            var json = JSONObject.Parse(HttpUtility.HtmlDecode(filter));
            return new SearchFilter(json);
        }

        public string Query { get; set; }

        public string TemplateName { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }

        public Id[] CategorizationIds { get; set; }

        public override string ToString()
        {
            var result = new JSONObject();
            if (!string.IsNullOrEmpty(Query))
                result.AddValue("q", Query);
            if (!string.IsNullOrEmpty(TemplateName))
                result.AddValue("t", TemplateName);
            if (From.HasValue)
                result.AddValue("df", From.Value);
            if (To.HasValue)
                result.AddValue("dt", To.Value);
            if (CategorizationIds != null && CategorizationIds.Any())
            {
                var cids = new JSONArray();
                cids.AddRange(CategorizationIds.Select(id => id.IntValue));
                result.AddValue("cids", cids);
            }
            return result.ToString();
        }

        
    }
}
