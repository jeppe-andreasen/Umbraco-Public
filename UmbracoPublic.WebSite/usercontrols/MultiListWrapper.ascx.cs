using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using umbraco.cms.businesslogic.datatype;
using umbraco.editorControls.userControlGrapper;

namespace UmbracoPublic.WebSite.usercontrols
{
    public partial class MultiListWrapper : System.Web.UI.UserControl, IUsercontrolDataEditor
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string src = "/handlers/MultiListEditorHandler.aspx?itemId=" + Request.QueryString["id"] + "&frame=" + gridEditorFrame.ClientID + "&hiddenId=" + hiddenValue.ClientID + "&provider=" + Provider + "&fieldname=" + FieldName;
            if (!string.IsNullOrEmpty(RootId))
                src += "&rootId=" + RootId;
            gridEditorFrame.Attributes.Add("src", src);
        }

        public object value
        {
            get { return hiddenValue.Value; }
            set { hiddenValue.Value = value.ToString(); }
        }

        [DataEditorSetting("Provider")]
        public string Provider { get; set; }

        [DataEditorSetting("FieldName")]
        public string FieldName { get; set; }

        public string RootId { get; set; }
    }
}