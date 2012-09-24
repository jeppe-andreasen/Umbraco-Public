using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using UmbracoPublic.Interfaces.Enumerations;
using UmbracoPublic.Interfaces.SiteManagement;
using UmbracoPublic.Logic.Entities;
using UmbracoPublic.Logic.Exceptions;
using UmbracoPublic.Logic.Utilities;
using umbraco.cms.businesslogic.web;

namespace UmbracoPublic.Logic.Controllers.SiteManagement
{
    public abstract class BaseSiteComponent : ISiteComponent
    {
        protected BaseSiteComponent()
        {
            this.Controls = new List<Control>();
        }

        public abstract string Name
        {
            get;
        }

        public SiteRoot SiteRoot { get; private set; }

        public void Initialize(Document siteRoot)
        {
            using (CmsContext.Editing)
            {
                SiteRoot = CmsService.Instance.GetItem<SiteRoot>(new Id(siteRoot.Id));
                try
                {
                    Initialize();    
                }
                catch(SiteComponentException exc)
                {
                    State = exc.State;
                    Controls.Add(new LiteralControl("<p>" + exc.Message + "</p>"));
                    if (exc.ButtonHandler != null)
                    {
                        var button = new Button();
                        button.Click += exc.ButtonHandler;
                        button.Text = exc.ButtonText;
                        button.CommandArgument = exc.CommandArgs;
                        AddButtons(button);
                    }
                }
                
            }
        }

        protected abstract void Initialize();

        public void InstantiateIn(ControlCollection controls)
        {
            foreach (var control in Controls)
                controls.Add(control);
        }

        protected List<Control> Controls { get; private set; } 

        public SiteComponentState State { get; protected set; }

        protected ConfigurationFolder GetSiteConfigurationFolder()
        {
            if (SiteRoot == null)
                throw new ArgumentNullException("The site root is null");
            return SiteRoot.GetChildrenOfType<ConfigurationFolder, GoBasicEntityTypeTable>().FirstOrDefault(n => n.EntityName == "Configuration");
        }

        protected ContentFolder GetSiteContentFolder()
        {
            if (SiteRoot == null)
                throw new ArgumentNullException("The site root is null");
            return SiteRoot.GetChildrenOfType<ContentFolder, GoBasicEntityTypeTable>().FirstOrDefault();
        }

        protected SystemLinkFolder GetSiteSystemLinkFolder(ConfigurationFolder folder = null)
        {
            if (folder == null)
                folder = GetSiteConfigurationFolder();
            if (folder == null)
                return null;
            return folder.GetChildrenOfType<SystemLinkFolder, GoBasicEntityTypeTable>().FirstOrDefault(n => n.EntityName == "SystemLinks");
        }

        

        protected Entities.SystemLink GetSiteSystemLink(string key, SystemLinkFolder folder = null)
        {
            if (folder == null)
                folder = GetSiteSystemLinkFolder();
            if (folder == null)
                return null;
            return folder.GetChildrenOfType<Entities.SystemLink, GoBasicEntityTypeTable>().FirstOrDefault(n => n.EntityName == key);
        }

        protected T GetSiteLinkedEntity<T>(SystemKey key, SystemLinkFolder folder = null) where T : Entity, new()
        {
            return GetSiteLinkedEntity<T>(key.ToString(), folder);
        }

        protected T GetSiteLinkedEntity<T>(string key, SystemLinkFolder folder = null) where T:Entity, new()
        {
            var systemLink = GetSiteSystemLink(key, folder);
            if (systemLink == null || systemLink.Link.IsNull)
                return null;
            return CmsService.Instance.GetItem<T>(systemLink.Link);
        }

        protected ConfigurationFolder EnsureSiteConfigurationFolder()
        {
            return GetSiteConfigurationFolder() ?? CmsService.Instance.CreateEntity<ConfigurationFolder>("Configuration", SiteRoot);
        }

        protected ContentFolder EnsureSiteContentFolder(ConfigurationFolder configurationFolder = null)
        {
            return GetSiteContentFolder() ?? CmsService.Instance.CreateEntity<ContentFolder>("Content", SiteRoot);
        }

        protected SystemLinkFolder EnsureSiteSystemLinkFolder(ConfigurationFolder configurationFolder = null)
        {
            var parent = configurationFolder ?? EnsureSiteConfigurationFolder();
            return GetSiteSystemLinkFolder(parent) ?? CmsService.Instance.CreateEntity<SystemLinkFolder>("SystemLinks", parent);
        }

        

        protected void AddMessage(string message)
        {
            Controls.Add(new LiteralControl("<p>" + message + "</p>"));
        }

        protected void AddButtons(params Button[] buttons)
        {
            Controls.Add(new LiteralControl("<div class=\"buttons\">"));
            foreach (var button in buttons)
                Controls.Add(button);
            Controls.Add(new LiteralControl("</div>"));
        }

        protected Entities.SystemLink EnsureSiteSystemLink(SystemKey key, Entity linkedEntity)
        {
            return EnsureSiteSystemLink(key.ToString(), linkedEntity);
        }

        protected Entities.SystemLink EnsureSiteSystemLink(string key, Entity linkedEntity)
        {
            var parent = EnsureSiteSystemLinkFolder();
            var result = GetSiteSystemLink(key, parent) ?? CmsService.Instance.CreateEntity<Entities.SystemLink>(key, parent);
            if (linkedEntity != null)
            {
                result.Link = linkedEntity.Id;
                result.Save();
                result.Publish();
            }
            return result;
        }

        protected void ReloadEditor(Entity target = null)
        {
            if (target == null)
                System.Web.HttpContext.Current.Response.Redirect(System.Web.HttpContext.Current.Request.Url.PathAndQuery);
            else
            {
                var page = umbraco.BasePages.BasePage.Current;
                page.ClientScript.RegisterStartupScript(page.GetType(), "refreshItem", "top.window.location.href = '/umbraco/umbraco.aspx?app=content&rightAction=editContent&id=" + (target != null ? target.Id : SiteRoot.Id) + "#content';", true);
            }
        }
    }
}
