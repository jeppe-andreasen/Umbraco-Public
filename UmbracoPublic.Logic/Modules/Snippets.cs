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
using UmbracoPublic.Logic.Modules.Contact;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.Logic.Modules
{
    public class Snippets
    {
        public static void RenderNewsResults(HtmlWriter writer, SearchRecord[] records, bool renderUl = true)
        {
            var categorizationLookup = CategorizationFolder.Get();
            var newsListUrl = Urls.GetMainNewsListUrl();

            var visibleCategorizations = CategorizationFolder.Get().Types.Where(t => !t.IsHidden).SelectMany(t => t.Items).Where(i => !i.IsHidden).ToDictionary(i => i.Id);

            if (renderUl)
                writer.RenderBeginTag(HtmlTextWriterTag.Ul, "results");
            foreach (var record in records)
            {
                var date = record.GetDate("date");
                if (date == null)
                    continue;

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
            if (renderUl)
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

        public static void RenderContact(HtmlWriter writer, ContactModule module, bool showHeader)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Div, "thumbnail contact");
            if (showHeader)
                writer.RenderFullTag(HtmlTextWriterTag.H3, "Kontakt");
            writer.AddAttribute(HtmlTextWriterAttribute.Src, module.Image.Url);
            writer.AddAttribute(HtmlTextWriterAttribute.Alt, module.FullName);
            writer.RenderImageTag(module.Image.Url, module.FullName, null);
            writer.RenderBeginTag(HtmlTextWriterTag.Div, "caption");
            writer.RenderBeginTag(HtmlTextWriterTag.H5);
            writer.Write(module.FullName);
            writer.RenderEndTag(); // h5
            writer.RenderBeginTag(HtmlTextWriterTag.P);
            WriteContactInfo(writer, module.Title);
            WriteContactInfo(writer, module.Area);
            if (!string.IsNullOrEmpty(module.Email))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Title, "Email : " + module.Email);
                writer.RenderLinkTag("mailto:" + module.Email, "Email : " + module.Email);
                writer.WriteBreak();
            }
            WriteContactInfo(writer, module.Phone, "Tlf.");
            WriteContactInfo(writer, module.Mobile, "Mobil");
            writer.RenderEndTag(); // p
            writer.RenderEndTag(); // div.caption
            writer.RenderEndTag(); // div.thumbnail.contact
        }

        private static void WriteContactInfo(LinqIt.Utils.Web.HtmlWriter writer, string info, string prefix = "")
        {
            if (!string.IsNullOrEmpty(info))
            {
                if (!string.IsNullOrEmpty(prefix))
                    writer.Write(prefix + " ");
                writer.Write(info);
                writer.WriteBreak();
            }
        }
    }
}
