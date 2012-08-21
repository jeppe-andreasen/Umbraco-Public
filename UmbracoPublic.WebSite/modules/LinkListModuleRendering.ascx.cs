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
    public partial class LinkListModuleRendering : BaseModule<LinkListModule>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void RenderModule(LinkListModule item, LinqIt.Utils.Web.HtmlWriter writer)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Ul,  "linklist");
            foreach (var link in item.Links)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Li);
                writer.RenderLink(link);
                writer.RenderEndTag();
            }
            writer.RenderEndTag();
        }


        public override int[] GetModuleColumnOptions()
        {
            return new int[]{3,4,6,9,12};
        }
    }
}