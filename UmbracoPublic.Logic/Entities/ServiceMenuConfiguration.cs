using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;
using umbraco.cms.businesslogic.web;

namespace UmbracoPublic.Logic.Entities
{
    public class ServiceMenuConfiguration : Entity
    {
        public LinkList Items
        {
            get { return GetValue<LinkList>("items"); }
        }

        public override Id TemplateId
        {
            get
            {
                return new Id(DocumentType.GetByAlias("ServiceMenuConfiguration").Id);
            }
        }

        public override string TemplatePath
        {
            get { return "Configuration/ServiceMenuConfiguration"; }
        }
    }
}
