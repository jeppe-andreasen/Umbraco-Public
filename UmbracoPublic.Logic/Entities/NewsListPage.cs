using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;
using umbraco.cms.businesslogic.web;

namespace UmbracoPublic.Logic.Entities
{
    public class NewsListPage : WebPage
    {
        public LinqIt.Cms.Data.Id[] CategorizationIds { get { return new Id[0];} }

        public bool ShowPager { get { return true; } }

        public int MaxItemsShown
        {
            get { return 10; }
        }

        public override Id TemplateId
        {
            get { return new Id(DocumentType.GetByAlias("NewsListPage").Id); }
        }

        public override string TemplatePath
        {
            get { return "/WebPage/NewsListPage"; }
        }
    }
}
