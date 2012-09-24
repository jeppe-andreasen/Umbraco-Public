using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;
using umbraco.cms.businesslogic.web;

namespace UmbracoPublic.Logic.Entities
{
    public class SiteSearchResultPage : WebPage
    {
        public int MaxItemsShown { get { return 20; } }

        public override Id TemplateId
        {
            get { return new Id(DocumentType.GetByAlias("SiteSearchResultPage").Id); }
        }

        public override string TemplatePath
        {
            get { return "/WebPage/SiteSearchResultPage"; }
        }
    }
}
