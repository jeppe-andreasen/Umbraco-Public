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
            writer.RenderBeginTag(HtmlTextWriterTag.Ul, "thumbnails");
            writer.RenderBeginTag(HtmlTextWriterTag.Li, "span3");

            if (!string.IsNullOrEmpty(item.Headline))
                writer.RenderFullTag(HtmlTextWriterTag.H3, item.Headline);

            foreach (var contact in item.Contacts)
                Snippets.RenderContact(writer, contact, null);

            writer.RenderEndTag(); // li.span3
            writer.RenderEndTag(); // ul.thumbnails




        }
    }
}
