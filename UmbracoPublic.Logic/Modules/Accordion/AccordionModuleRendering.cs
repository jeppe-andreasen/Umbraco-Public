using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using LinqIt.Components.Data;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.Logic.Modules.Accordion
{
    public class AccordionModuleRendering : BaseModuleRendering<AccordionModule>
    {
        protected override void OnRegisterScripts()
        {
            ModuleScripts.RegisterInitScript("accordion");
        }

        protected override void CreateChildControls()
        {
            HtmlContent.CreateModuleContent(RenderOutput, this.Controls, ColumnSpan);
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
                writer.AddClasses("accordion-body", "collapse");
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
            return new int[] { 3, 4, 6, 9, 12 };
        }

    }
}
