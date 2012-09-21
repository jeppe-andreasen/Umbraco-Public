using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;
using umbraco.cms.businesslogic.web;

namespace UmbracoPublic.Logic.Entities
{
    public class GridModuleFolder : Entity
    {
        public override Id TemplateId
        {
            get
            {
                return new Id(DocumentType.GetByAlias("GridModuleFolder").Id);
            }
        }

        public override string TemplatePath
        {
            get { return "/GridModuleFolder"; }
        }
    }
}
