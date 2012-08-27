using System;
using System.Web.UI;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using LinqIt.Components;
using LinqIt.UmbracoCustomFieldTypes;

namespace UmbracoPublic.WebSite.test
{
    public partial class GridEditorTest2 : System.Web.UI.Page
    {
        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            var itemId = new Id(Request.QueryString["itemid"]);
            var page = CmsService.Instance.GetItem<Entity>(itemId);
            var grideditor = new LinqItGridEditor();
            grideditor.GridItemProvider = typeof(UmbracoTreeModuleProvider).AssemblyQualifiedName;
            grideditor.ItemId = itemId.ToString();
            grideditor.Value = page["grid"];
            grideditor.Frame = Request.QueryString["frame"];
            grideditor.HiddenId = Request.QueryString["hiddenId"];
            Controls.Add(grideditor);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            Page.ClientScript.RegisterStartupScript(GetType(), "grideditorinitialization", "grideditor.editModule = function(moduleId){top.openContent(moduleId);};", true);
        }
    }
}