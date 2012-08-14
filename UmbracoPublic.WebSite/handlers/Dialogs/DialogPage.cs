using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UmbracoPublic.WebSite.handlers.Dialogs
{
    public class DialogPage : System.Web.UI.Page
    {
        public virtual bool ShowOk { get { return true; } }

        public virtual bool ShowCancel { get { return true; } }

        public virtual DialogResponse HandleOk()
        {
            return null;
        }
    }
}