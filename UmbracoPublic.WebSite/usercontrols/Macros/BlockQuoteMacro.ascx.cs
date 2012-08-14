using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UmbracoPublic.WebSite.usercontrols.Macros
{
    public partial class BlockQuoteMacro : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

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