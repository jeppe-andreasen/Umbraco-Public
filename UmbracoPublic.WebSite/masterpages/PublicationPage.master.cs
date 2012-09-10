using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Cms;
using LinqIt.Components;
using LinqIt.Utils.Web;
using UmbracoPublic.WebSite.Utilities;

namespace UmbracoPublic.WebSite.masterpages
{
    public partial class PublicationPage : System.Web.UI.MasterPage, IGridModuleControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            litContent.Text = HtmlWriter.Generate(GenerateOutput);
        }

        private static void GenerateOutput(HtmlWriter writer)
        {
            var page = CmsService.Instance.GetItem<UmbracoPublic.Logic.Entities.NewsPage>();
            if (!string.IsNullOrEmpty(page.Headline))
                writer.RenderFullTag(HtmlTextWriterTag.H1, page.Headline);
            if (page.Date.HasValue)
                writer.RenderFullTag(HtmlTextWriterTag.H6, string.Format("Publiseret {0}", page.Date.Value.ToString("dd-MM-yyyy")));
            foreach (var categorization in page.Categorizations)
                writer.RenderFullTag(HtmlTextWriterTag.Span, categorization, "label");
            if (!page.Intro.IsEmpty)
                writer.RenderParagraph(page.Intro.AsHtml);

            var linkedDocument = page.LinkedDocument;
            if (linkedDocument.Exists)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Div, "linked-document");
                writer.RenderBeginTag(HtmlTextWriterTag.Ul, "thumbnails");
                writer.RenderBeginTag(HtmlTextWriterTag.Li, "span4");
                writer.RenderBeginTag(HtmlTextWriterTag.Div, "thumbnail");
                writer.RenderImageTag(page.Thumbnail.Url, null, null);
                writer.RenderBeginTag(HtmlTextWriterTag.P);
                writer.RenderLinkTag(page.LinkedDocument.Url, "Åbn publikation", "btn btn-primary");
                writer.RenderEndTag(); // p
                writer.RenderEndTag(); // div.thumbnail
                writer.RenderEndTag(); // li.span4
                writer.RenderEndTag(); // ul.thumbnails
                writer.RenderEndTag();
            }
            if (!page.Body.IsEmpty)
                writer.Write(page.Body.ToString());
        }

        public GridLayout GetGridLayout()
        {
            return PageLayouts.GeneralLayout;
        }       
    }
}