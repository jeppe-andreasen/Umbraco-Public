using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;

namespace UmbracoPublic.Logic.Providers.MailProviders
{
    public class MailChimpConfiguration : Entity
    {
        public string ApiKey
        {
            get { return GetValue<string>("apiKey"); }
        }
    }
}
