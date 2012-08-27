using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;

namespace UmbracoPublic.Logic.Entities
{
    public class ServiceMenuConfiguration : Entity
    {
        public LinkList Items
        {
            get { return GetValue<LinkList>("items"); }
        }
    }
}
