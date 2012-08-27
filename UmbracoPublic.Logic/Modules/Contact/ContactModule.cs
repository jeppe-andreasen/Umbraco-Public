using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;
using LinqIt.Components.Data;

namespace UmbracoPublic.Logic.Modules.Contact
{
    public class ContactModule : BaseModule
    {
        public string Title { get { return GetValue<string>("title"); } }

        public string Area { get { return GetValue<string>("area"); } }

        public string Email { get { return GetValue<string>("email"); } }

        public string Phone { get { return GetValue<string>("phone"); } }

        public string Mobile { get { return GetValue<string>("mobile"); } }

        public Image Image { get { return GetValue<Image>("image"); } }

        public string FullName { get { return GetValue<string>("fullName"); } }
    }
}
