using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.Logic.Modules.Forms
{
    public class FormsModuleRendering : BaseModuleRendering<FormsModule>
    {
        private List<FieldSpecification> _specifications = new List<FieldSpecification>();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateMode = ViewStateMode.Enabled;
            EnsureChildControls();
        }

        protected override void RegisterScripts()
        {
            ModuleScripts.RegisterInitScript("forms");
        }

        public override string ModuleDescription
        {
            get { return "Module for simple forms"; }
        }

        protected override void CreateChildControls()
        {
            Controls.Add(new LiteralControl("<div class=\"form\">"));

            if (!string.IsNullOrEmpty(Module.Headline))
                Controls.Add(new LiteralControl("<h2>" + Module.Headline + "</h2>"));

            var validationGroup = "vg" + Module.Id;
            foreach (var field in Module.Fields)
            {
                var spec = new FieldSpecification(validationGroup, this.ColumnSpan.Value);
                Controls.Add(new LiteralControl("<div class=\"control-group\">"));
                field.PopulateSpecification(spec, this.Page.IsPostBack);

                foreach (var control in spec.Controls)
                    Controls.Add(control);    
                
                if (spec.Validators.Any())
                {
                    Controls.Add(new LiteralControl("<div class=\"validators\">"));        
                    
                    foreach (var validator in spec.Validators)
                        Controls.Add(validator);

                    Controls.Add(new LiteralControl("</div>"));
                }
                Controls.Add(new LiteralControl("</div>"));
            }

            var submit = new Button();
            submit.CssClass = "btn";
            submit.Click += OnSubmitClicked;
            submit.Text = "Send";
            submit.ValidationGroup = validationGroup;
            Controls.Add(submit);

            

            Controls.Add(new LiteralControl("</div>"));
            base.CreateChildControls();
        }

        void OnSubmitClicked(object sender, EventArgs e)
        {
            Page.Validate("vg" + Module.Id);
            if (Page.IsValid)
            {
                
            }
        }
    }
}
