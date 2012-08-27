using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.Logic.Macros
{
    public class ModuleMacro : Control
    {
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            if (string.IsNullOrEmpty(ModuleId))
                return;
            var control = HtmlContent.LoadModule(ModuleId, null);
            Controls.Add(control);
        }

        public string ModuleId { get; set; }
    }
}
