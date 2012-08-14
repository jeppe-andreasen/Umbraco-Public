using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using LinqIt.Cms.Data;
using LinqIt.Parsing.Css;
using LinqIt.Parsing.Html;
using LinqIt.Utils.Web;

namespace UmbracoPublic.Logic.Utilities
{
    public static class HtmlWriterExtensions
    {
        public static void RenderLink(this HtmlWriter writer, Link link)
        {
            if (!string.IsNullOrEmpty(link.Target))
                writer.AddAttribute(HtmlTextWriterAttribute.Target, link.Target);
            writer.RenderLinkTag(link.Href, link.Title, link.CssClass);

        }

        public static void RenderRichText(this HtmlWriter writer, Html html, HtmlTextWriterTag tag = HtmlTextWriterTag.Div, string cssClass = null)
        {
            writer.AddClass(cssClass);
            writer.RenderBeginTag(tag);

            var doc = new HtmlDocument(html.ToString());

            ProcessAlerts(doc);
            
            writer.Write(doc.ToString());
            writer.RenderEndTag();
        }

        private static void ProcessAlerts(HtmlDocument doc)
        {
            var cssQuery = CssSelectorStack.Parse(".alert");
            var tags = doc.FindElements(element => (element is HtmlTag) && element.Matches(cssQuery)).Cast<HtmlTag>();
            foreach (var t in tags.Where(t => !t.IsType("div")))
            {
                t.ChangeType("div");
                var text = t.InnerText;
                t.InnerHtml = string.Format("<button class=\"close\" data-dismiss=\"alert\">×</button><span>{0}</span>", text);
            }
        }

        public static void GoogleOff(this HtmlWriter writer)
        {
            writer.Write("<!--googleoff: index-->");
        }

        public static void GoogleOn(this HtmlWriter writer)
        {
            writer.Write("<!--googleon: index-->");
        }
    }
}
