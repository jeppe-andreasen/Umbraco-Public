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
        public static void RenderNewsResults(HtmlWriter writer, SearchRecord[] records)
        {
            var visibleCategorizations = CategorizationFolder.Get().Types.Where(t => !t.IsHidden).SelectMany(t => t.Items).Where(i => !i.IsHidden).ToDictionary(i => i.Id);
            foreach (var record in records)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.H3);
                writer.RenderLinkTag(record.GetString("url"), record.GetString("title"));
                writer.RenderEndTag();
                writer.RenderFullTag(HtmlTextWriterTag.H6, "Publiseret " + record.GetDate("date").Value.ToString("dd-MM-yyyy"));

                var categorizationIds = new IdList(record.GetString("categorizations"));
                
                foreach (var categorization in categorizationIds.Where(visibleCategorizations.ContainsKey).Select(id => visibleCategorizations[id]))
                    writer.RenderFullTag(HtmlTextWriterTag.Span, categorization.EntityName, "label");

                var text = record.GetString("summary");
                if (text.Length > 150)
                    text = text.Substring(0, 150);
                writer.RenderFullTag(HtmlTextWriterTag.P, text);
            }
        }
    }
}