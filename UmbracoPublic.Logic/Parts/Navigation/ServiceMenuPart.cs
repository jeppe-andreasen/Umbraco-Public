using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using LinqIt.Cms;
using UmbracoPublic.Logic.Entities;

namespace UmbracoPublic.Logic.Parts.Navigation
{
    public class ServiceMenuPart : BasePart
    {
        protected override void RenderPart(LinqIt.Utils.Web.HtmlWriter writer)
        {
            var configuration = CmsService.Instance.GetConfigurationItem<ServiceMenuConfiguration>("Service Menu");
            if (configuration == null)
                return;

            var items = configuration.Items;
            if (!items.Any())
                return;

            writer.RenderBeginTag(HtmlTextWriterTag.Div, "section");
            writer.RenderBeginTag(HtmlTextWriterTag.Ul, "nav nav-pills pull-right");
            foreach (var item in items)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Li);
                writer.AddAttribute(HtmlTextWriterAttribute.Href, item.Href);
                writer.RenderFullTag(HtmlTextWriterTag.A, item.Title);
                writer.RenderEndTag(); // li    
            }
            writer.RenderEndTag(); // ul.nav nav-pills pull-right
            writer.RenderEndTag(); // div.section

        }
    }
}
