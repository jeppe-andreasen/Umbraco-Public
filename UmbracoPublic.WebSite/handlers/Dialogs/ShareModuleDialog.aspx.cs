using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Ajax;
using LinqIt.Ajax.Parsing;
using LinqIt.Cms;
using LinqIt.Components.Data;
using LinqIt.UmbracoCustomFieldTypes;
using LinqIt.Utils.Extensions;
using umbraco.cms.businesslogic.web;

namespace UmbracoPublic.WebSite.handlers.Dialogs
{
    public partial class ShareModuleDialog : DialogPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (CmsContext.Editing)
            {
                treeview.Provider = typeof (GridLibraryFolderProvider).GetShortAssemblyName();
                treeview.ProviderReferenceId = Request.QueryString["pid"];
            }
            AjaxUtil.RegisterPageMethods(this);
        }

        [AjaxMethod(AjaxType.Sync)]
        public static JSONObject CreateNewFolder(string providerName, string referenceId, string parentId, string name)
        {
            var provider = ProviderHelper.GetTreeNodeProvider(providerName, referenceId);
            var documentType = DocumentType.GetByAlias("GridModuleFolder");
            var treeNode = provider.CreateNode(name, parentId, documentType.Id.ToString());
            var result = new JSONObject();
            result.AddValue("parentId", parentId);
            result.AddValue("addedId", treeNode.Id);
            return result;
        }

        public override DialogResponse HandleOk()
        {
            var response = new DialogResponse("ShareModule", true);
            var folderId = Convert.ToInt32(treeview.SelectedValue);
            var document = new Document(Convert.ToInt32(Request.QueryString["mid"]));
            document.Move(folderId);
            global::umbraco.library.UpdateDocumentCache(document.Id);
            response.AddValue("parentId", folderId);
            response.AddValue("moduleId", document.Id.ToString());
            return response;
        }
    }
}