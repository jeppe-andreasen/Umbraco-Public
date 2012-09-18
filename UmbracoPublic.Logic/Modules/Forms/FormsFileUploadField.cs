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
    public class FormsFileUploadField : FormsField
    {
        public override Id TemplateId
        {
            get { return new Id(DocumentType.GetByAlias("FormsFileUploadField").Id); }
        }

        public override string TemplatePath
        {
            get { return "/FormsField/FormsFileUploadField"; }
        }

        internal override void PopulateSpecification(FieldSpecification spec, bool isPostBack)
        {
            var upload = new FileUpload();
            upload.ID = "ui" + Id;
            upload.CssClass = spec.SpanClass;
            spec.AddControl(new LiteralControl("<div class=\"fileinputs\">"));
            spec.AddControl(GetLabel(upload));
            spec.AddControl(new LiteralControl("</div>"));
            spec.AddControl(upload);
            if (Mandatory)
                spec.AddValidator(GetRequiredFieldValidator(upload));
        }
    }
}
