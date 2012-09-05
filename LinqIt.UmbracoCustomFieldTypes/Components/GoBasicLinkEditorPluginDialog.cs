using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using LinqIt.Components;
using LinqIt.UmbracoCustomFieldTypes.Providers;
using LinqIt.Utils.Extensions;

namespace LinqIt.UmbracoCustomFieldTypes.Components
{
    public class GoBasicLinkEditorPluginDialog : System.Web.UI.Page
    {
        private LinqItLinkEditor _editor;
        protected PlaceHolder plhEditor;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            _editor = new LinqItLinkEditor();
            _editor.ID = "editor";
            _editor.Provider = typeof (GoBasicLinkEditorProvider).GetShortAssemblyName();
            _editor.ReferenceId = Request.QueryString["id"];
            plhEditor.Controls.Add(_editor);
        }
    }
}