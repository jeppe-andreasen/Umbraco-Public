using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;
using umbraco.cms.businesslogic.web;
using UmbracoPublic.Logic.Entities;

namespace UmbracoPublic.Logic.Modules.Forms
{
    public class FormsActionFolder : Entity
    {
        public override Id TemplateId
        {
            get { return new Id(DocumentType.GetByAlias("FormsActionFolder").Id); }
        }

        public override string TemplatePath
        {
            get { return "/FormsActionFolder"; }
        }

        public IEnumerable<FormsAction> Actions { get { return GetChildrenOfType<FormsAction, GoBasicEntityTypeTable>(); } }
    }
}
