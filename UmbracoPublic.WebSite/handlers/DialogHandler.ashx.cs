using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using LinqIt.Utils.Extensions;

namespace UmbracoPublic.WebSite.handlers
{
    /// <summary>
    /// Summary description for DialogHandler
    /// </summary>
    public class DialogHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            var dialog = context.Request.Form["dlg"];
            context.Response.Write("<iframe src='" + GetDialogUrl(dialog, context.Request.Form) + "' frameborder='0' style=\"width:100%;height:100%;\">");
        }

        private static string GetDialogUrl(string name, NameValueCollection parameters)
        {
            var result = new StringBuilder();

            if (name == "CustomContentEditor")
            {
                result.Append("/umbraco/CustomContentEditor.aspx");
            }
            else
            {
                result.Append("/handlers/Dialogs/");
                result.Append(name);
                result.Append("Dialog.aspx");
            }
            result.Append("?" + parameters.Keys.Cast<string>().ToSeparatedString("&", k => string.Format("{0}={1}", k, parameters[k])));
            return result.ToString();
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