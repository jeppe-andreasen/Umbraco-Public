using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using LinqIt.Ajax.Parsing;
using LinqIt.Utils.Extensions;
using LinqIt.Utils.Web;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.Logic.Parts
{
    public class BasePart : Control
    {
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            string content = HtmlWriter.Generate(RenderPart);
            if (!string.IsNullOrEmpty(content))
                writer.Write(content);
            base.Render(writer);
        }

        protected virtual void RenderPart(HtmlWriter writer)
        {
        }
    }
}