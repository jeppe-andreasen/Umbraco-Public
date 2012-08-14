using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Cms.Data;
using LinqIt.Utils.Web;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.WebSite.usercontrols.Macros
{
    public partial class FactsBoxMacro : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void CreateChildControls()
        {
            HtmlContent.CreateControls(RenderOutput, plhOutput);
            base.CreateChildControls();
        }

        private void RenderOutput(HtmlWriter writer)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Div,"well");
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