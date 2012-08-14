using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;

namespace UmbracoPublic.Logic.Entities
{
    public class LinkListModule : GridModule
    {
        public LinkList Links
        {
            get { return GetValue<LinkList>("links"); }
        }
    }
}
