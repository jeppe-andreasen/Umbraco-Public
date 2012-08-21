using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UmbracoPublic.Logic.Services;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.WebSite.usercontrols.Parts
{
    public partial class NavigationBar : BasePart
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void RenderPart(LinqIt.Utils.Web.HtmlWriter writer)
        {
            var topMenuItems = DataService.Instance.GetTopMenuItems();

            writer.RenderBeginTag(HtmlTextWriterTag.Div, "nav-collapse");
            writer.RenderBeginTag(HtmlTextWriterTag.Ul, "nav");

            foreach (var topItem in topMenuItems)
            {
                if (topItem.HasChildren)
                    writer.AddClass("dropdown");
                writer.RenderBeginTag(HtmlTextWriterTag.Li);
                writer.AddAttribute(HtmlTextWriterAttribute.Href, topItem.Url);
                writer.RenderFullTag(HtmlTextWriterTag.A, topItem.DisplayName);
                if (topItem.HasChildren)
                {
                    writer.AddAttribute("data-toggle", "dropdown");
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                    writer.RenderBeginTag(HtmlTextWriterTag.A, "dropdown-toggle");
                    writer.RenderFullTag(HtmlTextWriterTag.B, "", "caret");
                    writer.RenderEndTag(); // a.dropdown-toggle
                    writer.RenderBeginTag(HtmlTextWriterTag.Ul, "dropdown-menu");
                    foreach (var child in topItem.Children)
                    {
                        writer.RenderBeginTag(HtmlTextWriterTag.Li);
                        writer.AddAttribute(HtmlTextWriterAttribute.Href, child.Url);
                        writer.RenderFullTag(HtmlTextWriterTag.A, child.DisplayName);
                        writer.RenderEndTag(); // li
                    }
                    writer.RenderEndTag(); // ul.dropdown-menu
                }
                writer.RenderEndTag(); // li
            }
            writer.RenderEndTag(); // ul.nav
            writer.RenderEndTag(); // div.nav-collapse
        }


        //protected override void RenderPart(LinqIt.Utils.Web.HtmlWriter writer)
        //{
        //    var topMenuItems = DataService.Instance.GetTopMenuItems();
        //    writer.RenderBeginTag(HtmlTextWriterTag.Ul, "nav");
        //    foreach (var topItem in topMenuItems)
        //    {
        //        if (topItem.HasChildren)
        //        {
        //            writer.AddClass("dropdown");
        //            writer.RenderBeginTag(HtmlTextWriterTag.Li);
        //            writer.AddAttribute(HtmlTextWriterAttribute.Href, topItem.Url);
        //            writer.AddAttribute("data-toggle", "dropdown");
        //            writer.RenderBeginTag(HtmlTextWriterTag.A, "dropdown-toggle");
        //            writer.Write(topItem.DisplayName);
        //            writer.RenderFullTag(HtmlTextWriterTag.B, "", "caret");
        //            writer.RenderEndTag(); // a.dropdown-toggle    

        //            writer.RenderBeginTag(HtmlTextWriterTag.Ul, "dropdown-menu");
        //            foreach (var child in topItem.Children)
        //            {
        //                writer.RenderBeginTag(HtmlTextWriterTag.Li);
        //                writer.AddAttribute(HtmlTextWriterAttribute.Href, child.Url);
        //                writer.RenderFullTag(HtmlTextWriterTag.A, child.DisplayName);
        //                writer.RenderEndTag(); // li    
        //            }
        //            writer.RenderEndTag(); // ul.dropdown-menu
        //            writer.RenderEndTag(); // li.dropdown
        //        }
        //        else
        //        {
        //            writer.RenderBeginTag(HtmlTextWriterTag.Li);
        //            writer.AddAttribute(HtmlTextWriterAttribute.Href, topItem.Url);
        //            writer.RenderFullTag(HtmlTextWriterTag.A, topItem.DisplayName);
        //            writer.RenderEndTag(); // li
        //        }
        //    }
        //    writer.RenderEndTag(); // ul.nav
        //}
    }
}




