using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UmbracoPublic.Logic.Utilities
{
    public static class Paths
    {

        public static string Combine(params string[] parts)
        {
            var result = new StringBuilder();
            if (parts.Length > 0)
                result.Append(parts[0].TrimEnd('/'));
            for (var i = 1; i < parts.Length; i++)
            {
                result.Append('/');
                result.Append(parts[i].Trim('/'));
            }
            return result.ToString();
        }
    }
}
