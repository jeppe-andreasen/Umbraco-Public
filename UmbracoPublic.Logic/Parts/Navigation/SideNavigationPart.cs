using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using UmbracoPublic.Logic.Services;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.Logic.Parts.Navigation
{
    public class SideNavigationPart : BasePart
    {
        protected override void RenderPart(LinqIt.Utils.Web.HtmlWriter writer)
        {
            var menuItems = DataService.Instance.GetSideMenuItems();
            if (!menuItems.Any())
                return;

            writer.GoogleOff();

            writer.RenderBeginTag(HtmlTextWriterTag.Div, "well");
            writer.RenderBeginTag(HtmlTextWriterTag.Ul, "nav nav-list");

            foreach (var menuItem in menuItems)
                RenderMenuItem(writer, menuItem);

            writer.RenderEndTag(); // ul.nav nav-list
            writer.RenderEndTag(); // div.well

            writer.GoogleOn();
        }

        private void RenderMenuItem(LinqIt.Utils.Web.HtmlWriter writer, Entities.MenuItem menuItem)
        {
            if (menuItem.Active)
                writer.AddClass("active");

            writer.RenderBeginTag(HtmlTextWriterTag.Li);
            writer.AddAttribute(HtmlTextWriterAttribute.Href, menuItem.Url);
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.RenderFullTag(HtmlTextWriterTag.Span, menuItem.DisplayName);
            writer.RenderEndTag(); // a
            if (menuItem.HasChildren)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Ul);
                foreach (var child in menuItem.Children)
                    RenderMenuItem(writer, child);
                writer.RenderEndTag(); // ul
            }
            writer.RenderEndTag(); // li
        }
    }
}
