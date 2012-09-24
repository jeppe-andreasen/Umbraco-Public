using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UmbracoPublic.Logic.Modules.Forms
{
    public class FieldSpecification
    {
        private readonly string _validationGroup;
        private readonly int _span;
        private readonly List<Control> _controls = new List<Control>();
        private readonly List<BaseValidator> _validators = new List<BaseValidator>();

        public FieldSpecification(string validationGroup, int span)
        {
            _validationGroup = validationGroup;
            _span = span;
        }

        public string SpanClass
        {
            get { return "span" + _span; }
        }

        public void AddControl(Control control)
        {
            _controls.Add(control);
        }

        public void AddValidator(BaseValidator validator)
        {
            validator.CssClass = "validator";
            validator.Display = ValidatorDisplay.Dynamic;
            validator.ValidationGroup = _validationGroup;
            _validators.Add(validator);
        }
        public IEnumerable<Control> Controls { get { return _controls; } }

        public IEnumerable<BaseValidator> Validators { get { return _validators; } }

        public string Label { get; set; }

        public Func<object> Get { get; set; }
    }
}
