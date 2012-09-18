using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;
using umbraco.cms.businesslogic.web;
using UmbracoPublic.Logic.Entities;

namespace UmbracoPublic.Logic.Modules.Forms
{
    public class FormsFieldFolder : Entity
    {
        public override Id TemplateId
        {
            get { return new Id(DocumentType.GetByAlias("FormsFieldFolder").Id); }
        }

        public override string TemplatePath
        {
            get { return "/FormsFieldFolder"; }
        }

        public IEnumerable<FormsField> Fields { get { return GetChildrenOfType<FormsField, GoBasicEntityTypeTable>(); } }
    }
}
