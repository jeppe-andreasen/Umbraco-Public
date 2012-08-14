using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.WebSite.usercontrols.Macros
{
    public partial class ModuleMacro : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            if (string.IsNullOrEmpty(ModuleId))
                return;
            var control = HtmlContent.LoadModule(ModuleId, null);
            plhOutput.Controls.Add(control);
        }

        public string ModuleId { get; set; }
    }
}