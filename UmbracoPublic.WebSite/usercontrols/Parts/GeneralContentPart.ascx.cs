using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Cms;
using LinqIt.Utils.Web;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.WebSite.usercontrols.Parts
{
    public partial class GeneralContentPart : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void CreateChildControls()
        {
            HtmlContent.CreateControls(GenerateOutput, this.Controls);
            base.CreateChildControls();
        }

        private static void GenerateOutput(HtmlWriter writer)
        {
            var page = CmsService.Instance.GetItem<UmbracoPublic.Logic.Entities.WebPage>();
            if (!string.IsNullOrEmpty(page.Headline))
                writer.RenderFullTag(HtmlTextWriterTag.H1, page.Headline);
            if (!page.Intro.IsEmpty)
                writer.RenderFullTag(HtmlTextWriterTag.H2, page.Intro.AsHtml, "intro");
            if (!page.Body.IsEmpty)
                writer.Write(page.Body.ToString());
        }
    }
}