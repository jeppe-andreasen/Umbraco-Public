using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using LinqIt.Ajax.Parsing;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using LinqIt.Utils.Extensions;
using LinqIt.Utils.Web;
using UmbracoPublic.Interfaces;
using UmbracoPublic.Interfaces.Enumerations;
using UmbracoPublic.Logic.Entities;
using UmbracoPublic.Logic.Exceptions;
using umbraco.cms.businesslogic.web;

namespace UmbracoPublic.Logic.Services.Newsletters.MailChimp
{
    public class MailChimpServiceV13 : INewsletterService
    {
        private readonly MailChimpConfiguration _configuration;

        public MailChimpServiceV13()
        {
            _configuration = CmsService.Instance.GetConfigurationItem<MailChimpConfiguration>("MailChimp");
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

        public bool ValidateSiteConfiguration(int siteRootId, out string errorMessage)
        {
            var siteRoot = CmsService.Instance.GetItem<SiteRoot>(new Id(siteRootId));
            if (siteRoot == null)
            {
                errorMessage = "The site root could not be found";
                return false;
            }
            var configuration = CmsService.Instance.GetConfigurationItem<MailChimpConfiguration>("MailChimp", siteRoot.Path);

            if (configuration == null)
            {
                throw new SiteComponentException("A mail chimp configuration section is missing", SiteComponentState.Warning, OnFixConfigurationErrorClicked, "Add a mailchimp configuration item", siteRootId.ToString());
            }
            if (string.IsNullOrEmpty(configuration.ApiKey))
            {
                throw new SiteComponentException("Please fill out the mail chimp api key.", SiteComponentState.Warning, OnFixConfigurationErrorClicked, "Go to the mailchimp configuration", siteRootId.ToString());
            }

            errorMessage = string.Empty;
            return true;
        }

        private void OnFixConfigurationErrorClicked(object sender, EventArgs e)
        {
            using (CmsContext.Editing)
            {
                var button = (Button) sender;
                var siteRoot = CmsService.Instance.GetItem<SiteRoot>(new Id(button.CommandArgument));
                var configurationFolder = siteRoot.GetChildrenOfType<ConfigurationFolder>().FirstOrDefault(n => n.EntityName == "Configuration") ?? CmsService.Instance.CreateEntity<ConfigurationFolder>("Configuration", siteRoot);
                var mailchimp = configurationFolder.GetChildrenOfType<MailChimpConfiguration>().FirstOrDefault(n => n.EntityName == "MailChimp") ?? CmsService.Instance.CreateEntity<MailChimpConfiguration>("MailChimp", configurationFolder);
                var page = umbraco.BasePages.BasePage.Current;
                page.ClientScript.RegisterStartupScript(page.GetType(), "refreshItem", "top.window.location.href = '/umbraco/umbraco.aspx?app=content&rightAction=editContent&id=" + mailchimp.Id + "#content';", true);
            }
        }
    }
}
