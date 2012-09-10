using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;

namespace UmbracoPublic.Logic.Modules.GoogleAnalytics
{
    public class GoogleAnalyticsModule : Entity
    {
        public string ApiKey
        {
            get { return GetValue<string>("apiKey"); }
        }
    }
}
