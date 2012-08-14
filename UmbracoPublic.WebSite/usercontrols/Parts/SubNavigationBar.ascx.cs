using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UmbracoPublic.Logic.Services;

namespace UmbracoPublic.WebSite.usercontrols.Parts
{
    public partial class SubNavigationBar : BasePart
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void RenderPart(LinqIt.Utils.Web.HtmlWriter writer)
        {
            var subItems = DataService.Instance.GetSubMenuItems();
            if (!subItems.Any())
                return;

            writer.RenderBeginTag(HtmlTextWriterTag.Div, "subnav");
            writer.RenderBeginTag(HtmlTextWriterTag.Ul, "nav nav-pills");
            foreach (var menuItem in subItems)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Li);
                writer.RenderLinkTag(menuItem.Url, menuItem.DisplayName);
                writer.RenderEndTag(); // li
            }
            writer.RenderEndTag(); // ul.nav nav-pills
            writer.RenderEndTag(); // div.subnav
        }
    }
}