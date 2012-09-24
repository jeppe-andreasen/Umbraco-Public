using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms;
using UmbracoPublic.Interfaces.Enumerations;
using UmbracoPublic.Logic.Entities;
using UmbracoPublic.Logic.Exceptions;

namespace UmbracoPublic.Logic.Controllers.SiteManagement
{
    public class NewsletterComponent : BaseSiteComponent
    {
        public override string Name
        {
            get { return "Newsletters"; }
        }

        protected override void Initialize()
        {
            var configurationFolder = GetSiteConfigurationFolder();
            if (configurationFolder == null)
                throw new SiteComponentException("Newsletters have not been configured on this site", SiteComponentState.Disabled, AddNewsletterConfigurationClicked);

            var newsletterConfiguration = configurationFolder.GetChildrenOfType<NewsletterConfiguration>().FirstOrDefault(c => c.EntityName == "Newsletters");
            if (newsletterConfiguration == null)
                throw new SiteComponentException("Newsletters have not been configured on this site", SiteComponentState.Disabled, AddNewsletterConfigurationClicked);
            var newsletterService = newsletterConfiguration.NewsletterService;
            if (newsletterService == null)
                throw new SiteComponentException("A newsletter service has noot been selected", SiteComponentState.Warning, AddNewsletterConfigurationClicked);

            string errorMessage;
            if (!newsletterService.ValidateSiteConfiguration(SiteRoot.Id.IntValue, out errorMessage))
            {
                AddMessage("The newsletter service [" + newsletterService.DisplayName + "] reports the following configuration error.");
                throw new SiteComponentException(errorMessage, SiteComponentState.Warning, null);
            }
            AddMessage("Newsletters have been configured successfully");
        }

        private void AddNewsletterConfigurationClicked(object sender, EventArgs e)
        {
            var configurationFolder = EnsureSiteConfigurationFolder();
            var newsletterConfiguration = configurationFolder.GetChildrenOfType<NewsletterConfiguration>().FirstOrDefault(c => c.EntityName == "Newsletters");
            if (newsletterConfiguration == null)
                CmsService.Instance.CreateEntity<NewsletterConfiguration>("Newsletters", configurationFolder);
            ReloadEditor(newsletterConfiguration);
        }
    }
}
