using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Components;
using LinqIt.UmbracoCustomFieldTypes.Providers;
using LinqIt.Utils.Extensions;

namespace UmbracoPublic.WebSite.assets.lib.tiny_mce.plugins.linqitimage
{
    public partial class dialog : System.Web.UI.Page
    {
        private LinqItImageEditor _editor;
       
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            _editor = new LinqItImageEditor();
            _editor.ID = "editor";
            _editor.Provider = typeof(GoBasicImageEditorProvider).GetShortAssemblyName();
            _editor.ReferenceId = Request.QueryString["id"];
            plhEditor.Controls.Add(_editor);
        }
    }
}