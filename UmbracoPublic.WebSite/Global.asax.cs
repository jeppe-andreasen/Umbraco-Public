using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace UmbracoPublic.WebSite
{
    public class Global : global::umbraco.Global
    {
        protected override void Application_BeginRequest(object sender, EventArgs e)
        {
            base.Application_BeginRequest(sender, e);
        }

        protected virtual void Session_Start(object sender, EventArgs e)
        {

        }
    }
}