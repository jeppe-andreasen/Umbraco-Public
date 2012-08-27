using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using LinqIt.Utils.Web;

namespace UmbracoPublic.Logic.Macros
{
    public class AlertMacro : Control
    {
        public string Text { get; set; }

        public string Type { get; set; }

        protected override void Render(HtmlTextWriter w)
        {
            var writer = new HtmlWriter(w);
            writer.AddClass("alert");
            if (!string.IsNullOrEmpty(this.Type))
                writer.AddClass(this.Type);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.AddAttribute("data-dismiss", "alert");
            writer.RenderFullTag(HtmlTextWriterTag.Button, "×", "close");
            writer.Write(HttpUtility.UrlDecode(Text));
            writer.RenderEndTag(); // div.alert
        }
    }
}
