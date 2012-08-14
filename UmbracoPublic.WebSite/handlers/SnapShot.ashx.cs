using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using LinqIt.Cms;
using umbraco.cms.businesslogic.web;

namespace UmbracoPublic.WebSite.handlers
{
    /// <summary>
    /// Summary description for SnapShot
    /// </summary>
    public class SnapShot : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/xml";

            using (CmsContext.Editing)
            {
                var builder = new StringBuilder();
                using (var sw = new StringWriter(builder))
                using (var writer = new XmlTextWriter(sw))
                {
                    CmsService.Instance.BuildSnapShot(DateTime.Today.AddDays(-5), writer);    
                }
                context.Response.Write(builder.ToString());
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}