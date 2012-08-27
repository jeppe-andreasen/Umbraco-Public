using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace UmbracoPublic.Logic.Macros
{
    public class BlockQuoteMacro : Control
    {
        protected override void Render(HtmlTextWriter writer)
        {
            writer.RenderBeginTag("blockquote");
            writer.RenderBeginTag(HtmlTextWriterTag.P);
            writer.Write(Text);
            writer.RenderBeginTag(HtmlTextWriterTag.Small);
            writer.Write(Author);
            writer.RenderEndTag();
            writer.RenderEndTag();
            writer.RenderEndTag();
            base.Render(writer);
        }

        public string Text { get; set; }

        public string Author { get; set; }
    }
}
