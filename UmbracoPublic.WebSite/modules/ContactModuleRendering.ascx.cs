using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Utils.Web;
using UmbracoPublic.Logic.Entities;

namespace UmbracoPublic.WebSite.modules
{
    public partial class ContactModuleRendering : BaseModule<ContactModule>
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void RenderModule(ContactModule module, LinqIt.Utils.Web.HtmlWriter writer)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Ul, "thumbnails");
            writer.RenderBeginTag(HtmlTextWriterTag.Li, "span3");
            writer.RenderFullTag(HtmlTextWriterTag.H3, "Kontakt");
            writer.AddAttribute(HtmlTextWriterAttribute.Src, module.Image.Url);
            writer.AddAttribute(HtmlTextWriterAttribute.Alt, module.FullName);
            writer.RenderImageTag(module.Image.Url, module.FullName, null);
            writer.RenderBeginTag(HtmlTextWriterTag.Div, "caption");
            writer.RenderBeginTag(HtmlTextWriterTag.H5);
            writer.Write(module.FullName);
            writer.RenderEndTag(); // h5
            writer.RenderBeginTag(HtmlTextWriterTag.P);
            WriteInfo(writer, module.Title);
            WriteInfo(writer, module.Area);
            if (!string.IsNullOrEmpty(module.Email))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Title, "Email : " + module.Email);
                writer.RenderLinkTag("mailto:" + module.Email, "Email : " + module.Email);
                writer.WriteBreak();
            }
            WriteInfo(writer, module.Phone, "Tlf.");
            WriteInfo(writer, module.Mobile, "Mobil");
            writer.RenderEndTag(); // p
            writer.RenderEndTag(); // div.caption
            writer.RenderEndTag(); // li.span3
            writer.RenderEndTag(); // ul.thumbnails
        }

        private static void WriteInfo(LinqIt.Utils.Web.HtmlWriter writer, string info, string prefix = "")
        {
            if (!string.IsNullOrEmpty(info))
            {
                if (!string.IsNullOrEmpty(prefix))
                    writer.Write(prefix + " ");
                writer.Write(info);
                writer.WriteBreak();
            }
        }
    }
}