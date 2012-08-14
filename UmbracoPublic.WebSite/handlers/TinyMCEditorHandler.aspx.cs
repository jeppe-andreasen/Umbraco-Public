using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UmbracoPublic.WebSite.handlers
{
    public partial class TinyMCEditorHandler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.TinyMCEditor1.HiddenId = Request.QueryString["hiddenId"];
            this.TinyMCEditor1.Width = new Unit(300, UnitType.Pixel);
        }
    }
}