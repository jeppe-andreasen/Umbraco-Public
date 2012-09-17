using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using UmbracoPublic.Logic.Services;

namespace UmbracoPublic.Logic.Parts.Navigation
{
    public class BreadCrumbPart : BasePart
    {
        protected override void RenderPart(LinqIt.Utils.Web.HtmlWriter writer)
        {
            var items = DataService.Instance.GetBreadCrumbItems().ToArray();



            writer.RenderBeginTag(HtmlTextWriterTag.Ul, "breadcrumb");
            writer.RenderFullTag(HtmlTextWriterTag.Li, "Du er her:");
            
            for (var i = 0; i < items.Length; i++)
            {
                if (i == items.Length-1)
                {
                    writer.RenderFullTag(HtmlTextWriterTag.Li, items[i].DisplayName, "active");
                }
                else
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, items[i].Url);
                    writer.RenderFullTag(HtmlTextWriterTag.A, items[i].DisplayName);
                    writer.RenderFullTag(HtmlTextWriterTag.Span, "/", "divider");        
                }
                
            }
            writer.RenderEndTag(); // ul.breadcrumb
        }
    }
}
