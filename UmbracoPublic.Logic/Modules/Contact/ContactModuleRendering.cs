using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace UmbracoPublic.Logic.Modules.Contact
{
    public class ContactModuleRendering : BaseModuleRendering<ContactModule>
    {
        protected override void RenderModule(ContactModule module, LinqIt.Utils.Web.HtmlWriter writer)
        {
            Snippets.RenderContact(writer, module, true);
        }

        public override string ModuleDescription
        {
            get { return "Kontaktoplysning"; }
        }
    }
}
