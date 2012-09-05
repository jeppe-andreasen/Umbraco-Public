using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Components.Data;
using umbraco.cms.businesslogic.web;
using UmbracoPublic.Logic.Providers.MailProviders;

namespace UmbracoPublic.WebSite.test
{
    public partial class MailChimpTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var t1 = DateTime.Now;
            var provider = new MailChimpProviderV1_3();
            var t2 = DateTime.Now;
            foreach (var list in provider.GetLists())
            {
                litOutput.Text += "<p>" +  list.DisplayName + "</p>";
            }
            var t3 = DateTime.Now;

            litOutput.Text += "<p>T1: " + t2.Subtract(t1).TotalMilliseconds + "msec</p>";
            litOutput.Text += "<p>T1: " + t3.Subtract(t2).TotalMilliseconds + "msec</p>";


        }
    }
}