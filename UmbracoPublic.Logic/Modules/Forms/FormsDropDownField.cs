using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Cms.Data;
using umbraco.cms.businesslogic.web;

namespace UmbracoPublic.Logic.Modules.Forms
{
    public class FormsDropDownField : FormsField
    {
        public string[] Values
        {
            get 
            { 
                var value = GetValue<string>("values");
                if (string.IsNullOrEmpty(value))
                    return new string[0];
                
                var result = new List<string>();
                using (var reader = new StringReader(value))
                {
                    var line = reader.ReadLine();
                    while (line != null)
                    {
                        result.Add(line);
                        line = reader.ReadLine();
                    }
                }
                return result.ToArray();
            }
        }

        internal override void PopulateSpecification(FieldSpecification spec, bool isPostBack)
        {
            var dropDownList = new DropDownList();
            dropDownList.ID = "ui" + Id;
            dropDownList.Items.Add(new ListItem(string.Empty));
            dropDownList.CssClass = spec.SpanClass;
            foreach (var value in Values)
                dropDownList.Items.Add(value);

            spec.AddControl(GetLabel(dropDownList));
            spec.AddControl(dropDownList);
            if (Mandatory)
            {
                var validator = GetRequiredFieldValidator(dropDownList);
                validator.InitialValue = dropDownList.Items[0].Value;
                spec.AddValidator(validator);
            }

            spec.Get = () => dropDownList.SelectedValue;
        }

        public override Id TemplateId
        {
            get { return new Id(DocumentType.GetByAlias("FormsDropDownField").Id); }
        }

        public override string TemplatePath
        {
            get { return "/FormsField/FormsDropDownField"; }
        }
    }
}
