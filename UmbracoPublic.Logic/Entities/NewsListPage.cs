using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;

namespace UmbracoPublic.Logic.Entities
{
    public class NewsListPage : WebPage
    {
        public LinqIt.Cms.Data.Id[] CategorizationIds { get { return new Id[0];} }

        public bool ShowPager { get { return true; } }

        public int MaxItemsShown
        {
            get { return 10; }
        }
    }
}
