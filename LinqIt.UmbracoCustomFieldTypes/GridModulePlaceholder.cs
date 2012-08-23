using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using LinqIt.Components.Bootstrap;
using LinqIt.Utils.Extensions;

namespace LinqIt.UmbracoCustomFieldTypes
{
    [DefaultProperty("Provider")]
    [ToolboxData("<{0}:GridModulePlaceholder runat=server></{0}:GridModulePlaceholder>")]
    [ParseChildren(true)]
    public class GridModulePlaceholder : Control, INamingContainer
    {
        public GridModulePlaceholder()
        {
        }

        [Browsable(false), DefaultValue(null), Description("The header template."), TemplateContainer(typeof(GridModuleSection)), PersistenceMode(PersistenceMode.InnerProperty)]
        public virtual ITemplate HeaderTemplate { get; set; }

        [Browsable(false), DefaultValue(null), Description("The footer template."), TemplateContainer(typeof(GridModuleSection)), PersistenceMode(PersistenceMode.InnerProperty)]
        public virtual ITemplate FooterTemplate { get; set; }


        [Bindable(true), Category("Configuration"), DefaultValue(""), Description(""), Localizable(false)]
        public virtual string Key { get { return (string)ViewState["Key"] ?? string.Empty; } set { ViewState["Key"] = value; } }

        [Bindable(true), Category("Configuration"), DefaultValue(""), Description(""), Localizable(false)]
        public virtual string ItemPath { get { return (string)ViewState["ItemPath"] ?? string.Empty; } set { ViewState["ItemPath"] = value; } }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            
            if (HeaderTemplate != null)
            {
                var header = new GridModuleSection();
                HeaderTemplate.InstantiateIn(header);
                Controls.AddAt(0, header);
            }

            var placeholder = new BootstrapGridModulePlaceholder();
            placeholder.Provider = typeof(UmbracoTreeModuleProvider).GetShortAssemblyName();

            if (!string.IsNullOrEmpty(ItemPath))
            {
                var itemPath = ItemPath.StartsWith("§") ? CmsService.Instance.GetSystemPath(ItemPath.TrimStart('§')) : ItemPath;
                placeholder.ReferenceId = CmsService.Instance.GetItem<Entity>(itemPath).Id.ToString();
            }
            else
                placeholder.ReferenceId = CmsService.Instance.GetItem<Entity>().Id.ToString();
            placeholder.Key = Key;
            Controls.Add(placeholder);

            if (FooterTemplate != null)
            {
                var footer = new GridModuleSection();
                FooterTemplate.InstantiateIn(footer);
                Controls.Add(footer);
            }
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            

            
        }
    }

    [ToolboxItem(false)]
    public class GridModuleSection : Control, INamingContainer
    {
    }


}
