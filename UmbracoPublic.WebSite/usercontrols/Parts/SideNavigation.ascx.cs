using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UmbracoPublic.Logic.Parts;
using UmbracoPublic.Logic.Services;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.WebSite.usercontrols.Parts
{
    public partial class SideNavigation : BaseUCPart
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected override void RenderPart(LinqIt.Utils.Web.HtmlWriter writer)
        {
            writer.GoogleOff();

            var menuItems = DataService.Instance.GetSideMenuItems();
            if (!menuItems.Any())
                return;

            writer.AddAttribute(HtmlTextWriterAttribute.Style, "padding: 8px 0;");
            writer.RenderBeginTag(HtmlTextWriterTag.Div, "well");
            writer.RenderBeginTag(HtmlTextWriterTag.Ul, "nav nav-list");

            foreach (var menuItem in menuItems)
                RenderMenuItem(writer, menuItem);

            writer.RenderEndTag(); // ul.nav nav-list
            writer.RenderEndTag(); // div.well

            writer.GoogleOn();
        }

        private static void RenderMenuItem(LinqIt.Utils.Web.HtmlWriter writer, Logic.Entities.MenuItem menuItem)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Li, menuItem.Active? "active" : null);
            writer.RenderLinkTag(menuItem.Url, menuItem.DisplayName);
            if (menuItem.HasChildren)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Ul);
                foreach (var child in menuItem.Children)
                    RenderMenuItem(writer, child);
                writer.RenderEndTag();
            }
            writer.RenderEndTag(); //li
        }
    }
}