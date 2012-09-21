using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using UmbracoPublic.Interfaces.Enumerations;
using UmbracoPublic.Logic.Entities;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.Logic.Controllers.SiteManagement
{
    public class ModuleComponent : BaseSiteComponent
    {
        private CheckBox _cbSiteModuleFolder;
        private CheckBox _cbGlobalModuleFolder;

        public override string Name
        {
            get { return "Module System"; }
        }

        protected override void Initialize()
        {
            var siteModuleFolder = GetSiteLinkedEntity<GridModuleFolder>(SystemKey.ModuleFolder);
            var globalModuleFolder = GetSiteLinkedEntity<GlobalGridModuleFolder>(SystemKey.GlobalModuleFolder);
            bool addButton = false;
            if (siteModuleFolder == null && globalModuleFolder == null)
            {
                State = SiteComponentState.Disabled;
                AddMessage("No module folders have been configured for this site.");
                AddSiteModuleFolderCheckBox();
                AddGlobalModuleFolderCheckBox();
                addButton = true;
            }
            else
            {
                
                AddMessage("The module system has been configured");
                if (siteModuleFolder != null)
                    AddMessage("Modules specific to " + SiteRoot.EntityName + " are located here: [" + siteModuleFolder.Path + "]");
                else
                {
                    AddSiteModuleFolderCheckBox();
                    addButton = true;
                }
                if (globalModuleFolder != null)
                    AddMessage("Modules shared with other sites in " + SiteRoot.GetParent<Entity>().EntityName +" are located here: [" + globalModuleFolder.Path + "]");
                else
                {
                    AddGlobalModuleFolderCheckBox();
                    addButton = true;
                }
            }
            if (addButton)
            {
                var button = new Button();
                button.Text = "Update";
                button.Click += OnUpdateClicked;

                AddButtons(button);
            }
        }

        private void AddSiteModuleFolderCheckBox()
        {
            _cbSiteModuleFolder = AddCheckBox("cbSiteModuleFolder", "Add a site-specific module folder");
        }

        private void AddGlobalModuleFolderCheckBox()
        {
            _cbGlobalModuleFolder = AddCheckBox("cbGlobalModuleFolder", "Add a global module folder, for sharing modules between sites.");
        }

        private CheckBox AddCheckBox(string id, string text)
        {
            Controls.Add(new LiteralControl("<div class=\"checkbox\">"));
            var result = new CheckBox();
            result.Text = text;
            result.ID = id;
            Controls.Add(result);
            Controls.Add(new LiteralControl("</div>"));
            return result;
        }

        void OnUpdateClicked(object sender, EventArgs e)
        {
            using (CmsContext.Editing)
            {
                if (_cbSiteModuleFolder != null && _cbSiteModuleFolder.Checked)
                {
                    var contentFolder = EnsureSiteContentFolder();
                    CreateModuleFolder<GridModuleFolder>("Site Modules", contentFolder, SystemKey.ModuleFolder);
                }
                if (_cbGlobalModuleFolder != null && _cbGlobalModuleFolder.Checked)
                {
                    var companyFolder = SiteRoot.GetParent<Entity>();
                    CreateModuleFolder<GlobalGridModuleFolder>("Global Modules", companyFolder, SystemKey.GlobalModuleFolder);
                }
                ReloadEditor();
            }
        }

        private void CreateModuleFolder<T>(string name, Entity parent, SystemKey key) where T:Entity, new()
        {
            var moduleFolder = CmsService.Instance.SelectItemsOfType<T>(parent.Path + "/*").FirstOrDefault();
            if (moduleFolder == null)
                moduleFolder = CmsService.Instance.CreateEntity<T>(name, parent);
            EnsureSiteSystemLink(key, moduleFolder);
        }
    }
}
