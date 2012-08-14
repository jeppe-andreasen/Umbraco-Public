using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;

namespace UmbracoPublic.Logic.Entities
{
    public class DisqusConfiguration : Entity
    {
        public bool DeveloperMode { get { return GetValue<bool>("developerMode"); } }

        public string SiteName { get { return GetValue<string>("siteName"); } }
    }

    
}
