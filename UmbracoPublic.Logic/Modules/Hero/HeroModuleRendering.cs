using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.Logic.Modules.Hero
{
    public class HeroModuleRendering : BaseModuleRendering<HeroModule>
    {
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
