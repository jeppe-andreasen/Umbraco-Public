using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace UmbracoPublic.Logic.Modules.ContactList
{
    public class ContactListModuleRendering : BaseModuleRendering<ContactListModule>
    {
        protected override void RenderModule(ContactListModule item, LinqIt.Utils.Web.HtmlWriter writer)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Div, "contact-list");
            if (!string.IsNullOrEmpty(item.Headline))
                writer.RenderFullTag(HtmlTextWriterTag.H3, item.Headline);

            writer.RenderBeginTag(HtmlTextWriterTag.Ul, "thumbnails");
            foreach (var contact in item.Contacts)
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Li, "span3");
                Snippets.RenderContact(writer, contact, false);
                writer.RenderEndTag(); // li.span3
            }
            writer.RenderEndTag(); // ul.thumbnails
            writer.RenderEndTag(); // div.contact-list
        }
    }
}
