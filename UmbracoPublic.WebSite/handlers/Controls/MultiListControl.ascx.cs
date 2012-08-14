using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Components.Data;
using LinqIt.Utils.Web;

namespace UmbracoPublic.WebSite.handlers.Controls
{
    public partial class MultiListControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Initialize(Control sourceControl, IEnumerable<Node> selectedNodes)
        {
            plhSrc.Controls.Add(sourceControl);
            litDstBox.Text = GenerateListBox(selectedNodes);
        }



        public string GenerateListBox(IEnumerable<Node> nodes)
        {
            return HtmlWriter.Generate(writer =>
            {
                foreach (var node in nodes)
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Li);
                    writer.AddAttribute("ref", node.Id);
                    writer.AddAttribute(HtmlTextWriterAttribute.Title, node.HelpText);
                    writer.AddClass("item");
                    writer.RenderBeginLink("#");
                    writer.RenderImageTag(node.Icon, null, null);
                    writer.RenderFullTag(HtmlTextWriterTag.Span, node.Text);
                    writer.RenderEndTag(); // a
                    writer.RenderEndTag(); // li
                }
            });
        }
    
    }
}