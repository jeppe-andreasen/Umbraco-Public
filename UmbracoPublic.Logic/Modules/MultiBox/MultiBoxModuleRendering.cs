using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.Logic.Modules.MultiBox
{
    public class MultiBoxModuleRendering : BaseModuleRendering<MultiBoxModule>
    {
        protected override void CreateChildControls()
        {
            HtmlContent.CreateControls(RenderOutput, this.Controls);
            base.CreateChildControls();
        }

        protected void RenderOutput(LinqIt.Utils.Web.HtmlWriter writer)
        {
            if (Module == null)
                return;

            if (!string.IsNullOrEmpty(Module.Headline))
            {
                writer.RenderBeginTag(HtmlTextWriterTag.H2);
                if (Module.Link != null && !string.IsNullOrEmpty(Module.Link.Href))
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Href, Module.Link.Href);
                    writer.RenderFullTag(HtmlTextWriterTag.A, Module.Headline);
                }
                else
                    writer.Write(Module.Headline);
                writer.RenderEndTag(); // h2
            }
            writer.RenderParagraph(Module.Intro.AsHtml);
            
            writer.RenderBeginTag(HtmlTextWriterTag.Div, "thumbnail");
            if (Module.Image.Exists)
                writer.RenderImageTag(Module.Image.Url, Module.Headline, null);
            writer.RenderRichText(Module.Body, HtmlTextWriterTag.Div, "caption");
            writer.RenderEndTag(); // div.thumbnail
        }

        public override string ModuleDescription
        {
            get { return "Indholdsboks"; }
        }
    }
}
