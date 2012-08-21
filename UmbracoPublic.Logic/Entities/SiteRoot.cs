using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;

namespace UmbracoPublic.Logic.Entities
{
    public class SiteRoot : Entity
    {
        public string Theme
        {
            get
            {
                var theme = GetValue<string>("theme");
                if (string.IsNullOrEmpty(theme))
                    theme = "Default";
                return theme;
            }
        }
    }
}
