using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using LinqIt.Cms.Data;
using LinqIt.Utils.Web;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.Logic.Macros
{
    public class FactsBoxMacro : Control
    {
        protected override void CreateChildControls()
        {
            HtmlContent.CreateControls(RenderOutput, this.Controls);
            base.CreateChildControls();
        }

        private void RenderOutput(HtmlWriter writer)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Div, "well facts");
            if (!string.IsNullOrEmpty(Headline))
                writer.RenderFullTag(HtmlTextWriterTag.H3, Headline);
            var html = new Html(System.Web.HttpUtility.UrlDecode(Content));
            writer.RenderRichText(html);
            writer.RenderEndTag();
        }

        public string Headline { get; set; }

        public string Content { get; set; }
    }
}
