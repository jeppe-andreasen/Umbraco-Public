using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;

namespace UmbracoPublic.Logic.Entities
{
    public class SiteSearchResultPage : WebPage
    {
        public int MaxItemsShown { get { return 20; } }
    }
}
