using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms;
using UmbracoPublic.Interfaces.Enumerations;
using UmbracoPublic.Logic.Entities;
using UmbracoPublic.Logic.Exceptions;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.Logic.Controllers.SiteManagement
{
    public class NewsArchiveComponent : BaseSiteComponent
    {
        public override string Name
        {
            get { return "News Archive"; }
        }

        protected override void Initialize()
        {
            var newsArchivePage = GetSiteLinkedEntity<NewsListPage>(SystemKey.NewsArchivePage);
            if (newsArchivePage == null)
            {
                var homeItem = GetSiteLinkedEntity<WebPage>(SystemKey.HomePage);
                if (homeItem == null)
                {
                    AddMessage("The News archive is not yet enabled.");
                    throw new SiteComponentException("Please fix any home item issues before setting up search", SiteComponentState.Disabled);
                }
                throw new SiteComponentException("The News archive is not yet enabled.", SiteComponentState.Disabled, OnSetupClicked, "Enable news archive");
            }
            AddMessage("The News archive has been setup correctly.");
        }

        protected void OnSetupClicked(object sender, EventArgs e)
        {
            using (CmsContext.Editing)
            {
                var homeItem = GetSiteLinkedEntity<WebPage>(SystemKey.HomePage);
                var resultPage = CmsService.Instance.SelectSingleItemOfType<NewsListPage>(homeItem.Path + "//*{NewsListPage}");
                if (resultPage == null)
                    resultPage = CmsService.Instance.CreateEntity<NewsListPage>("News Archive", homeItem);
                EnsureSiteSystemLink(SystemKey.NewsArchivePage, resultPage);
            }
            ReloadEditor();

        }
    }
}
