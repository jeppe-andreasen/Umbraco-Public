using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UmbracoPublic.WebSite.handlers.Dialogs
{
    public partial class MasterDialog : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var page = this.Page as DialogPage;
                if (page != null)
                {
                    btnOk.Visible = page.ShowOk;
                    btnCancel.Visible = page.ShowCancel;
                }
            }
        }

        protected void OnOkClicked(object sender, EventArgs e)
        {
            var page = this.Page as DialogPage;
            if (page == null)
                throw new ApplicationException("Dialog " + this.Page.GetType().Name + " must inherit from DialogPage.");

            if (!page.IsValid)
                return;

            var response = page.HandleOk();
            if (response == null)
                throw new ApplicationException("Dialog " + page.GetType().Name + " must override OnOkClicked.");
            page.ClientScript.RegisterStartupScript(page.GetType(), "close", "closedialog(" + response.ToJSON() + ");", true);
        }
    }
}