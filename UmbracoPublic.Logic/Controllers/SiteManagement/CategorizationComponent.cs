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
    public class CategorizationComponent : BaseSiteComponent
    {
        public override string Name
        {
            get { return "Categorization"; }
        }

        protected override void Initialize()
        {
            var categorizationFolder = GetSiteLinkedEntity<CategorizationFolder>(SystemKey.CategorizationFolder);
            if (categorizationFolder == null)
                throw new SiteComponentException("Categorization is not yet enabled for this site.", SiteComponentState.Disabled, SetupCategorization, "Enable categorization");

            AddMessage("Categorization has been setup correctly.");
            AddMessage("Please go to [" + categorizationFolder.Path + "] to add more categories.");
        }

        

        protected void SetupCategorization(object sender, EventArgs e)
        {
            using (CmsContext.Editing)
            {
                var contentFolder = EnsureSiteContentFolder();

                var categorizationFolder = contentFolder.GetChildrenOfType<CategorizationFolder, GoBasicEntityTypeTable>().FirstOrDefault();
                if (categorizationFolder == null)
                    categorizationFolder = CmsService.Instance.CreateEntity<CategorizationFolder>("Categorizations", contentFolder);
                
                EnsureSiteSystemLink(SystemKey.CategorizationFolder, categorizationFolder);
                ReloadEditor(categorizationFolder);
            }
            
        }
    }
}
