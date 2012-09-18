using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;
using umbraco.cms.businesslogic.web;

namespace UmbracoPublic.Logic.Modules.Forms
{
    public class FormsActionFolder : Entity
    {
        public override Id TemplateId
        {
            get { return new Id(DocumentType.GetByAlias("FormsActionFolder").Id); }
        }
    }
}
