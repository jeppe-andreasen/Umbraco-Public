using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Cms;
using UmbracoPublic.Interfaces.Enumerations;
using UmbracoPublic.Logic.Entities;
using UmbracoPublic.Logic.Exceptions;
using UmbracoPublic.Logic.Utilities;
using umbraco.cms.businesslogic.web;

namespace UmbracoPublic.Logic.Controllers.SiteManagement
{
    public class HomePageComponent : BaseSiteComponent
    {
        public override string Name
        {
            get { return "Home Page"; }
        }

        protected override void Initialize()
        {
            var home = GetSiteLinkedEntity<LinqIt.Cms.Data.Page>(SystemKey.HomePage);
            if (home == null)
                throw new SiteComponentException("The home page is missing or has not been configured.", SiteComponentState.Warning, FixHomePageClicked, "Setup home item");

            var domains = umbraco.library.GetCurrentDomains(home.Id.IntValue);
            var hostName = domains != null ? domains.Select(n => n.Name).FirstOrDefault() : null;
            if (string.IsNullOrEmpty(hostName))
                throw new SiteComponentException("The home item does not have a hostname. Please right click on the home item [" + home.Path + "], select 'Manage Hostnames, and fill out the form", SiteComponentState.Warning, null);

            AddMessage("The home page is configured correctly.");
        }

        private void FixHomePageClicked(object sender, EventArgs e)
        {
            using (CmsContext.Editing)
            {
                var rootPages = SiteRoot.GetChildrenOfType<WebPage, GoBasicEntityTypeTable>().ToArray();
                WebPage home = null;
                if (rootPages.Count() == 1)
                    home = rootPages.First();
                else if (rootPages.Count() > 1)
                    home = rootPages.FirstOrDefault(p => p.EntityName == "Home");
                if (home == null)
                    home = CmsService.Instance.CreateEntity<WebPage>("Home", SiteRoot);

                EnsureSiteSystemLink(SystemKey.HomePage, home);
            }
            ReloadEditor();
        }
    }
}
