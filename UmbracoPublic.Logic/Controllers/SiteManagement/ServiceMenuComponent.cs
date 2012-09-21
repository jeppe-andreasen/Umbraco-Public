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
    public class ServiceMenuComponent : BaseSiteComponent
    {
        public override string Name
        {
            get { return "Service Menu"; }
        }

        protected override void Initialize()
        {
            var configurationFolder = GetSiteConfigurationFolder();
            if (configurationFolder != null)
            {
                var serviceMenuConfiguration = configurationFolder.GetChildrenOfType<ServiceMenuConfiguration, GoBasicEntityTypeTable>().FirstOrDefault(s => s.EntityName == "Service Menu");
                if (serviceMenuConfiguration != null)
                {
                    AddMessage("The Service menu has been setup correctly.");
                    AddMessage("Please go to [" + serviceMenuConfiguration.Path + "] to add items to the menu.");
                    return;
                }
            }
            throw new SiteComponentException("Service menu is not yet enabled for this site.", SiteComponentState.Disabled, OnSetupClicked, "Enable service menu");
        }

        

        protected void OnSetupClicked(object sender, EventArgs e)
        {
            using (CmsContext.Editing)
            {
                var configurationFolder = EnsureSiteConfigurationFolder();
                var serviceMenu = CmsService.Instance.CreateEntity<ServiceMenuConfiguration>("Service Menu", configurationFolder);
                ReloadEditor(serviceMenu);
            }
        }
    }
}