using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using LinqIt.Search;
using LinqIt.Utils.Web;
using UmbracoPublic.Logic.Entities;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.Logic.Modules
{
    public class Snippets
    {
        public static void RenderNewsResults(HtmlWriter writer, SearchRecord[] records)
        {
            var categorizationLookup = CategorizationFolder.Get();
            var newsListUrl = Urls.GetMainNewsListUrl();

            var visibleCategorizations = CategorizationFolder.Get().Types.Where(t => !t.IsHidden).SelectMany(t => t.Items).Where(i => !i.IsHidden).ToDictionary(i => i.Id);
            writer.RenderBeginTag(HtmlTextWriterTag.Ul);
            foreach (var record in records)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Li, "clearfix");

                if (!string.IsNullOrEmpty(record.GetString("thumbnail")))
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    writer.RenderImageTag(record.GetString("thumbnail"), record.GetString("title"), null);
                    writer.RenderEndTag(); // div

                }


                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.AddAttribute(HtmlTextWriterAttribute.Href, record.GetString("url"));
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.RenderFullTag(HtmlTextWriterTag.H3, record.GetString("title"));
                writer.RenderEndTag(); // a
                writer.RenderFullTag(HtmlTextWriterTag.Span, "Publiseret " + record.GetDate("date").Value.ToString("dd-MM-yyyy"), "date");

                var categorizations = new IdList(record.GetString("categorizations"));
                if (categorizations.Any())
                {
                    RenderCategorizations(writer, categorizations, categorizationLookup, newsListUrl);
                }
                writer.RenderBeginTag(HtmlTextWriterTag.A);

                var text = record.GetString("summary");
                if (text.Length > 150)
                    text = text.Substring(0, 150);
                writer.RenderFullTag(HtmlTextWriterTag.P, text);

                writer.RenderEndTag(); // a
                writer.RenderEndTag(); // div
                writer.RenderEndTag(); // li.clearfix
            }
            writer.RenderEndTag();
        }

        public static void RenderCategorizations(HtmlWriter writer, IEnumerable<Id> categorizations, CategorizationFolder allCategorizations = null, string newsListUrl = null)
        {
            if (newsListUrl == null)
                newsListUrl = Urls.GetMainNewsListUrl();
            if (allCategorizations == null)
                allCategorizations = CategorizationFolder.Get();

            foreach (var categorizationId in categorizations)
            {
                if (!allCategorizations.HasCategorization(categorizationId))
                    continue;

                writer.RenderLinkTag(newsListUrl + "?categorizations=" + categorizationId, allCategorizations.GetCategorization(categorizationId).DisplayName, "label");
            }
        }
    }
}
