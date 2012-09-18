using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Cms.Data;

namespace UmbracoPublic.Logic.Modules.Forms
{
    public class FormsField : Entity
    {
        private Label _label;

        public string Label { get { return GetValue<string>("label"); } }

        public bool Mandatory { get { return GetValue<bool>("mandatory"); } }

        public string MandatoryErrorMessage { get { return GetValue<string>("mandatoryErrorMessage"); } }

        public string HelpText { get { return GetValue<string>("helpText"); } }

        //public virtual IEnumerable<Control> GetControls(bool isPostback, string validationGroup)
        //{
        //    return new Control[0];
        //}

        public override string TemplatePath
        {
            get { return "/FormsField"; }
        }

        protected Label GetLabel(Control control)
        {
            var label = new Label();
            label.CssClass = "control-label";
            label.Text = Label;
            label.ViewStateMode = ViewStateMode.Disabled;
            if (control != null)
                label.AssociatedControlID = control.ID;
            return label;
        }

        protected RequiredFieldValidator GetRequiredFieldValidator(Control control)
        {
            var requiredFieldValidator = new RequiredFieldValidator();
            requiredFieldValidator.ControlToValidate = control.ID;
            requiredFieldValidator.SetFocusOnError = true;
            requiredFieldValidator.ErrorMessage = MandatoryErrorMessage;
            return requiredFieldValidator;
        }

        internal virtual void PopulateSpecification(FieldSpecification spec, bool isPostBack)
        {
        }
    }
}
