using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using LinqIt.Cms.Data;
using LinqIt.Search;
using LinqIt.Utils.Web;
using UmbracoPublic.Logic.Entities;
using UmbracoPublic.Logic.Services;
using UmbracoPublic.WebSite.usercontrols.Parts;

namespace UmbracoPublic.WebSite.Utilities
{
    public class Snippets
    {
        public static void RenderNewsResults(HtmlWriter writer, SearchResult result, Pager pager)
        {
            var categorizations = DataService.Instance.GetCategorizations();

            foreach (var record in result.Records.OrderByDescending(r => r.GetDate("date")).Skip(pager.Skip).Take(pager.Take))
            {
                writer.RenderBeginTag(HtmlTextWriterTag.H3);
                writer.RenderLinkTag(record.GetString("url"), record.GetString("title"));
                writer.RenderEndTag();
                writer.RenderFullTag(HtmlTextWriterTag.H6, "Publiseret " + record.GetDate("date").Value.ToString("dd-MM-yyyy"));
                var categorizationList = record.GetString("categorizations");
                if (!string.IsNullOrEmpty(categorizationList))
                {
                    foreach (var categorizationId in categorizationList.Split(',').Select(s => new Id(s.Trim())).Where(categorizations.ContainsKey))
                    {
                        writer.RenderFullTag(HtmlTextWriterTag.Span, categorizations[categorizationId], "label");
                    }
                }

                string text = record.GetString("summary");
                if (text.Length > 150)
                    text = text.Substring(0, 150);
                writer.RenderFullTag(HtmlTextWriterTag.P, text);
            }
        }
    }
}