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
            writer.RenderBeginTag(HtmlTextWriterTag.Ul, "linklist");
            foreach (var link in item.Links)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Li);
                writer.RenderLink(link);
                writer.RenderEndTag();
            }
            writer.RenderEndTag();
        }
    }
}
