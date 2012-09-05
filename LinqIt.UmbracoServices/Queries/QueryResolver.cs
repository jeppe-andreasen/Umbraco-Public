using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;

namespace LinqIt.UmbracoServices.Queries
{
    public class QueryResolver
    {
        public IEnumerable<Entity> Find(string query)
        {
            var parts = query.TrimStart('/').Split('/');
            Queue<Entity> queue = new Queue<Entity>();
            return null;

        }
    }
}
