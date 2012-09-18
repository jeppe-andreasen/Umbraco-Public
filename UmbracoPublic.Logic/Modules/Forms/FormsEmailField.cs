using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Cms.Data;
using umbraco.cms.businesslogic.web;

namespace UmbracoPublic.Logic.Modules.Forms
{
    public class FormsEmailField : FormsField
    {

        public override Id TemplateId
        {
            get { return new Id(DocumentType.GetByAlias("FormsEmailField").Id); }
        }

        public override string TemplatePath
        {
            get { return "/FormsField/FormsEmailField"; }
        }

        public string InvalidEmailErrorMessage
        {
            get { return GetValue<string>("invalidEmailErrorMessage"); }
        }

        internal override void PopulateSpecification(FieldSpecification spec, bool isPostBack)
        {
            var textBox = new TextBox();
            textBox.ID = "ui" + Id;
            textBox.MaxLength = 150;
            textBox.CssClass = spec.SpanClass;

            spec.AddControl(GetLabel(textBox));
            spec.AddControl(textBox);
            if (Mandatory)
                spec.AddValidator(GetRequiredFieldValidator(textBox));

            var regularExpressionValidator = new RegularExpressionValidator();
            regularExpressionValidator.ValidationExpression = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            regularExpressionValidator.ControlToValidate = textBox.ID;
            regularExpressionValidator.ErrorMessage = InvalidEmailErrorMessage;
            spec.AddValidator(regularExpressionValidator);
        }
    }
}
