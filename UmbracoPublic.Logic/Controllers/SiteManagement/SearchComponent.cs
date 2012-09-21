using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using LinqIt.Cms;
using UmbracoPublic.Interfaces.Enumerations;
using UmbracoPublic.Logic.Entities;
using UmbracoPublic.Logic.Exceptions;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.Logic.Controllers.SiteManagement
{
    public class SearchComponent : BaseSiteComponent
    {
        public override string Name
        {
            get { return "Site Search"; }
        }

        protected override void Initialize()
        {
            var searchResultPage = GetSiteLinkedEntity<SiteSearchResultPage>(SystemKey.SiteSearchResultPage);
            if (searchResultPage == null)
            {
                Controls.Add(new LiteralControl("<p>Site search is not yet enabled</p>"));

                var homeItem = GetSiteLinkedEntity<WebPage>(SystemKey.HomePage);
                if (homeItem == null)
                    throw new SiteComponentException("Please fix any home item issues before setting up search", SiteComponentState.Disabled);

                throw new SiteComponentException("Site search has not been configured for this site.", SiteComponentState.Disabled, SetupSearch, "Enable search");
            }
            AddMessage("Site search has been setup correctly.");    
        }

        protected void SetupSearch(object sender, EventArgs e)
        {
            using (CmsContext.Editing)
            {
                var homeItem = GetSiteLinkedEntity<WebPage>(SystemKey.HomePage);
                var resultPage = CmsService.Instance.SelectSingleItemOfType<SiteSearchResultPage>(homeItem.Path + "//*{SiteSearchResultPage}");
                if (resultPage == null)
                    resultPage = CmsService.Instance.CreateEntity<SiteSearchResultPage>("Search", homeItem);
                EnsureSiteSystemLink(SystemKey.SiteSearchResultPage, resultPage);
            }
            ReloadEditor();
            
        }
    }
}
