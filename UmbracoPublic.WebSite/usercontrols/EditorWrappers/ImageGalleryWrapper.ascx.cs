using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using umbraco.editorControls.userControlGrapper;

namespace UmbracoPublic.WebSite.usercontrols.EditorWrappers
{
    public partial class ImageGalleryWrapper : System.Web.UI.UserControl, IUsercontrolDataEditor
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            gridEditorFrame.Attributes.Add("src", "/handlers/ImageGallery/Handler.aspx?itemId=" + Request.QueryString["id"] + "&frame=" + gridEditorFrame.ClientID + "&hiddenId=" + hiddenValue.ClientID);
        }

        public object value
        {
            get { return hiddenValue.Value; }
            set { hiddenValue.Value = value.ToString(); }
        }
    }
}