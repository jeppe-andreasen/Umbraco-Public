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
    public class GoBasicMacroEditorPluginDialog : System.Web.UI.Page
    {
        protected PlaceHolder plhEditor;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            var list = new DropDownList();
            plhEditor.Controls.Add(list);
        }
    }
}