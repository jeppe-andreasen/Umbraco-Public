using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using LinqIt.Components;
using LinqIt.Components.Data;
using LinqIt.Utils.Extensions;
using LinqIt.Utils.Web;
using UmbracoPublic.Logic.Entities;
using UmbracoPublic.Logic.Utilities;


namespace UmbracoPublic.WebSite.modules
{
    public partial class AccordionModuleRendering : BaseModule<AccordionModule>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ModuleScripts.RegisterInitScript("accordion");
        }

        protected override void CreateChildControls()
        {
            HtmlContent.CreateModuleContent(RenderOutput, plhOutput, ColumnSpan);
            base.CreateChildControls();
        }

        private void RenderOutput(LinqIt.Utils.Web.HtmlWriter writer)
        {
            if (Module == null)
                return;
            
            RenderAccordion(writer, Module.Data.Items, this.ClientID);
        }

        private static void RenderAccordion(LinqIt.Utils.Web.HtmlWriter writer, IEnumerable<AccordionItem> items, string parentId)
        {
            var accordionId = parentId + "acc";

            writer.AddAttribute(HtmlTextWriterAttribute.Id, accordionId);
            writer.RenderBeginTag(HtmlTextWriterTag.Div, "accordion");

            bool openFirst = false;

            var n = 1;
            foreach (var item in items)
            {
                var groupId = accordionId + "_g" + n;
                writer.RenderBeginTag(HtmlTextWriterTag.Div, "accordion-group");
                writer.RenderBeginTag(HtmlTextWriterTag.Div, "accordion-heading");
                writer.AddAttribute("data-toggle", "collapse");
                writer.AddAttribute("data-parent", "#" + accordionId);
                writer.AddAttribute(HtmlTextWriterAttribute.Href, "#" + groupId);
                writer.RenderFullTag(HtmlTextWriterTag.A, item.Headline, "accordion-toggle");
                writer.RenderEndTag(); // div.accordion-heading
                writer.AddAttribute(HtmlTextWriterAttribute.Id, groupId);
                writer.AddClasses("accordion-body","collapse");
                if (n == 1 && openFirst)
                    writer.AddClass("in");
                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.RenderBeginTag(HtmlTextWriterTag.Div, "accordion-inner");
                if (!string.IsNullOrEmpty(item.Content))
                    writer.Write(item.Content);
                if (!string.IsNullOrEmpty(item.ModuleId))
                {
                    writer.AddAttribute("ref", item.ModuleId);
                    writer.RenderFullTag("module");
                }

                if (item.Items.Any())
                    RenderAccordion(writer, item.Items, groupId);

                writer.RenderEndTag();
                writer.RenderEndTag(); // div#collapseOne
                writer.RenderEndTag(); // div.accordion-group
                n++;
            }

            writer.RenderEndTag(); // div.accordion
        }

        public override int[] GetModuleColumnOptions()
        {
            return new int[]{3,4,6,9,12};
        }
    }
}