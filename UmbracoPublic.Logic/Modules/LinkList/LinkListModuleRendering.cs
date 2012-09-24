using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.Logic.Modules.LinkList
{
    public class LinkListModuleRendering : BaseModuleRendering<LinkListModule>
    {
        protected override void RenderModule(LinkListModule item, LinqIt.Utils.Web.HtmlWriter writer)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Div, "linklist");
            if (!string.IsNullOrEmpty(Module.Headline))
                writer.RenderFullTag(HtmlTextWriterTag.H2, Module.Headline);
            if (item.Links.Any())
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Ul);
                foreach (var link in item.Links)
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Li);
                    writer.RenderLink(link);
                    writer.RenderEndTag();
                }
                writer.RenderEndTag();
            } // ul
            writer.RenderEndTag(); // div.linklist
        }

        public override string ModuleDescription
        {
            get { return "Liste med links"; }
        }
    }
}
