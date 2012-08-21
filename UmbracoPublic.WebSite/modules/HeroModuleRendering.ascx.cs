using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UmbracoPublic.Logic.Entities;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.WebSite.modules
{
    public partial class HeroModuleRendering : BaseModule<HeroModule>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void RenderModule(HeroModule item, LinqIt.Utils.Web.HtmlWriter writer)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Div, "hero-unit");
            if (!string.IsNullOrEmpty(item.Headline))
                writer.RenderFullTag(HtmlTextWriterTag.H1, item.Headline);
            if (!item.Body.IsEmpty)
                writer.RenderRichText(item.Body);
            
            writer.RenderEndTag(); // div.hero-unit
        }
    }
}