using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace UmbracoPublic.Interfaces
{
    public interface INewsletterService
    {
        string DisplayName { get; }
        IEnumerable<MailingList> GetLists(); 
        bool SubscribeToList(string listId, string email, NameValueCollection userDetails);
        bool UnsubscribeToList(string listId, string email);
        bool ValidateSiteConfiguration(int siteRootId, out string errorMessage);
    }
}
