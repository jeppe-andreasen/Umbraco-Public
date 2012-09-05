using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using LinqIt.Ajax.Parsing;
using LinqIt.Cms;
using LinqIt.Utils.Extensions;
using LinqIt.Utils.Web;
using UmbracoPublic.Interfaces;

namespace UmbracoPublic.Logic.Providers.MailProviders
{
    public class MailChimpProviderV1_3 : IMailProvider
    {
        private readonly MailChimpConfiguration _configuration;

        public MailChimpProviderV1_3()
        {
            _configuration = CmsService.Instance.GetConfigurationItem<MailChimpConfiguration>("Mail Chimp");
        }


        public string DisplayName
        {
            get { return "MailChimp Version 1.3"; }
        }

        public IEnumerable<MailingList> GetLists()
        {
            var parameters = new JSONObject();
            parameters.AddValue("apikey", _configuration.ApiKey);
            var result = CallMethod("lists", parameters);
            var count = (int) result["total"];
            if (count == 0)
                return new MailingList[0];
            var data = (JSONArray) result["data"];

            return data.Values.Cast<JSONObject>().Select(o => new MailingList((string)o["id"], HttpUtility.HtmlDecode((string)o["name"]))).ToArray();
        }

        public string GetListsTmp()
        {
            var parameters = new JSONObject();
            parameters.AddValue("apikey", _configuration.ApiKey);
            return CallMethod("lists", parameters).ToString();
        }

        public bool SubscribeToList(string listId, string email, System.Collections.Specialized.NameValueCollection userDetails)
        {
            var parameters = new JSONObject();
            parameters.AddValue("apikey", _configuration.ApiKey);
            parameters.AddValue("id", listId);
            parameters.AddValue("email_address", email);

            var result = CallMethod("listSubscribe", parameters);
            return true;
        }

        public bool UnsubscribeToList(string listId, string email)
        {
            throw new NotImplementedException();
        }

        private JSONObject CallMethod(string method, JSONObject parameters)
        {
            var url = string.Format("http://{0}.api.mailchimp.com/1.3/?method={1}", _configuration.ApiKey.Split('-')[1], method);

            var response = HttpRequestUtil.Post(url, parameters.ToString());
            if (string.IsNullOrEmpty(response))
                return null;

            response = new Regex(@"\\u[0-9a-fA-F]{4}").ReplaceMatches(response, m => RemoveUnicode(m.Value));
            return JSONObject.Parse(response);
        }

        private string RemoveUnicode(string value)
        {
            var n = UInt16.Parse(value.Substring(2), NumberStyles.AllowHexSpecifier);
            var c = (Char) n;
            return c.ToString();
        }
    }
}
