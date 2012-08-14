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
using LinqIt.Utils.Extensions;
using LinqIt.Utils.Web;
using UmbracoPublic.Logic.Entities;
using UmbracoPublic.Logic.Utilities;
using umbraco.cms.businesslogic.macro;
using umbraco.cms.businesslogic.web;
using umbraco.cms.presentation.create.controls;
using umbraco.NodeFactory;

namespace UmbracoPublic.WebSite.modules
{
    public partial class MultiBoxModuleRendering : BaseModule<MultiBoxModule>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public override int[] GetModuleColumnOptions()
        {
            return new []{ 3, 4, 6, 9, 12 };
        }

        protected override void CreateChildControls()
        {
            HtmlContent.CreateControls(RenderOutput, plhOutput);
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
            writer.RenderBeginTag(HtmlTextWriterTag.Ul, "thumbnails");
            writer.RenderBeginTag(HtmlTextWriterTag.Li, "span" + ColumnSpan);
            writer.RenderBeginTag(HtmlTextWriterTag.Div, "thumbnail");
            if (Module.Image.Exists)
                writer.RenderImageTag(Module.Image.Url, Module.Headline, null);
            writer.RenderRichText(Module.Body, HtmlTextWriterTag.Div, "caption");
            //writer.RenderFullTag(HtmlTextWriterTag.Div, item.Body.ToString(), "caption");
            writer.RenderEndTag(); // div.thumbnail
            writer.RenderEndTag(); // li.spanX
            writer.RenderEndTag(); // ul.thumbnails
        }
    }
}