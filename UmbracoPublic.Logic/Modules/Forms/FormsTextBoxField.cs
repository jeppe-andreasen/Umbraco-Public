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
    public class FormsTextBoxField : FormsField
    {
        public override Id TemplateId
        {
            get { return new Id(DocumentType.GetByAlias("FormsTextBoxField").Id); }
        }

        public override string TemplatePath
        {
            get { return "/FormsField/FormsTextBoxField"; }
        }

        public int? MaxLength { get { return GetValue<int?>("maxLength"); } }

        public bool MultiLine
        {
            get { return GetValue<bool>("multiline"); }
        }

        internal override void PopulateSpecification(FieldSpecification spec, bool isPostBack)
        {
            var textBox = new TextBox();
            textBox.ID = "ui" + Id;
            textBox.CssClass = spec.SpanClass;
            if (MultiLine)
                textBox.TextMode = TextBoxMode.MultiLine;
            if (MaxLength.HasValue)
                textBox.MaxLength = MaxLength.Value;

            spec.AddControl(GetLabel(textBox));
            spec.AddControl(textBox);
            if (Mandatory)
                spec.AddValidator(GetRequiredFieldValidator(textBox));

            spec.Get = () => textBox.Text;
        }

        
    }
}
