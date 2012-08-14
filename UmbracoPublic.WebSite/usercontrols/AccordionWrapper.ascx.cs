using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using umbraco.editorControls.userControlGrapper;

namespace UmbracoPublic.WebSite.usercontrols
{
    public partial class AccordionWrapper : System.Web.UI.UserControl, IUsercontrolDataEditor
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            gridEditorFrame.Attributes.Add("src", "/handlers/AccordionEditorHandler.aspx?itemId=" + Request.QueryString["id"] + "&frame=" + gridEditorFrame.ClientID + "&hiddenId=" + hiddenValue.ClientID);
        }

        public object value
        {
            get { return hiddenValue.Value; }
            set { hiddenValue.Value = value.ToString(); }
        }
    }
}