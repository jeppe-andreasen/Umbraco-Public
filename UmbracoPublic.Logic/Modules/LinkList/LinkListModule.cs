using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;

namespace UmbracoPublic.Logic.Modules.LinkList
{
    public class LinkListModule : BaseModule
    {
        public LinqIt.Cms.Data.LinkList Links
        {
            get { return GetValue<LinqIt.Cms.Data.LinkList>("links"); }
        }
    }
}
