using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms;
using LinqIt.Cms.Data;

namespace UmbracoPublic.Logic.Entities
{
    public class NewsPage : WebPage
    {
        public DateTime? Date
        {
            get { return GetValue<DateTime?>("date"); }
        }

        public string[] Subjects
        {
            get
            {
                var value = this["subjects"];
                if (string.IsNullOrEmpty(value))
                    return new string[0];

                return new IdList(value).Select(id => CmsService.Instance.GetItem<Entity>(id)).Where(e => e != null).Select(e => e.EntityName).ToArray();
            }
        }
    }
}
