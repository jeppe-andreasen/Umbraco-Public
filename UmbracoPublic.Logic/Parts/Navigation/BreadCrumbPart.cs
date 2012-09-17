using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using UmbracoPublic.Logic.Entities;
using UmbracoPublic.Logic.Services;

namespace UmbracoPublic.Logic.Parts.Navigation
{
    public class BreadCrumbPart : BasePart
    {
        private MenuItem[] _menuItems;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            _menuItems = DataService.Instance.GetBreadCrumbItems().ToArray();
            Visible = _menuItems.Length > 1;
        }

        protected override void RenderPart(LinqIt.Utils.Web.HtmlWriter writer)
        {
            if (!Visible)
                return;

            writer.RenderBeginTag(HtmlTextWriterTag.Ul, "breadcrumb");
            writer.RenderFullTag(HtmlTextWriterTag.Li, "Du er her:");
            
            for (var i = 0; i < _menuItems.Length; i++)
            {
                if (i == _menuItems.Length-1)
                {
                    writer.RenderFullTag(HtmlTextWriterTag.Li, _menuItems[i].DisplayName, "active");
                }
                else
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, _menuItems[i].Url);
                    writer.RenderFullTag(HtmlTextWriterTag.A, _menuItems[i].DisplayName);
                    writer.RenderFullTag(HtmlTextWriterTag.Span, "/", "divider");        
                }
                
            }
            writer.RenderEndTag(); // ul.breadcrumb
        }
    }
}
