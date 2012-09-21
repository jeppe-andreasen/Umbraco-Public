using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using LinqIt.Cms.Data;
using umbraco.cms.businesslogic.web;

namespace UmbracoPublic.Logic.Modules.Forms
{
    public class FormsCheckBoxField : FormsField
    {
        public bool DefaultChecked { get { return GetValue<bool>("defaultChecked"); } }

        public override Id TemplateId
        {
            get { return new Id(DocumentType.GetByAlias("FormsCheckBoxField").Id); }
        }

        public override string TemplatePath
        {
            get { return "/FormsField/FormsCheckBoxField"; }
        }

        internal override void PopulateSpecification(FieldSpecification spec, bool isPostBack)
        {
            var label = new HtmlGenericControl("label");
            label.Attributes.Add("class", "checkbox");

            var checkbox = new CheckBox();
            checkbox.ID = "ui" + Id;
            if (!isPostBack)
                checkbox.Checked = DefaultChecked;
            label.Controls.Add(checkbox);
            label.Controls.Add(new LiteralControl(Label));
            spec.AddControl(label);

            spec.Get = () => checkbox.Checked;
        }
    }
}
