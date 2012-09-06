using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UmbracoPublic.Logic.Modules.Contact;

namespace UmbracoPublic.Logic.Modules.ContactList
{
    public class ContactListModule : BaseModule
    {
        public string Headline
        {
            get { return GetValue<string>("headline"); }
        }

        public IEnumerable<ContactModule> Contacts
        {
            get { return GetEntities<ContactModule>("contacts"); }
        }
    }
}
