using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UmbracoPublic.Interfaces
{
    public class MailingList
    {
        public MailingList(string id, string displayName)
        {
            Id = id;
            DisplayName = displayName;
        }
        
        public string DisplayName { get; private set; }
        public string Id { get; private set; }
    }
}
