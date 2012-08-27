using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Utils.Web;

namespace UmbracoPublic.Logic.Parts
{
    public class BaseUCPart : System.Web.UI.UserControl
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
