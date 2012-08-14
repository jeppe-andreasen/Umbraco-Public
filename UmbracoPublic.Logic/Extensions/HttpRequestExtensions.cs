using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using UmbracoPublic.Logic.Utilities;


namespace LinqIt.Utils.Extensions
{
    public static class HttpRequestExtensions
    {
        public static T GetQueryStringValue<T>(this HttpRequest request, QueryStringKey key, T defaultValue = default(T))
        {
            return LinqIt.Utils.Extensions.NameValueCollectionExtensions.GetValue(request.QueryString, key.ToString(),defaultValue);
        }
    }
}
