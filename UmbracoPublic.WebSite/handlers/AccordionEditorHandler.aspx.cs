using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Ajax;
using LinqIt.Ajax.Parsing;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using LinqIt.Components;
using LinqIt.Components.Data;
using LinqIt.UmbracoCustomFieldTypes;
using LinqIt.Utils.Extensions;

namespace UmbracoPublic.WebSite.handlers
{
    public partial class AccordionEditorHandler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxUtil.RegisterPageMethods(this);

            ClientScript.RegisterStartupScript(Page.GetType(), "editorInitialization", @"
            var gridEditorFrame = $('#" + Request.QueryString["frame"] + @"', window.parent.document);
            gridEditorFrame.parent().css('clear','both');
            ", true);
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            using (CmsContext.Editing)
            {
                var itemId = new Id(Request.QueryString["itemid"]);
                if (!Page.IsPostBack)
                {
                    hiddenId.Value = Request.QueryString["hiddenId"];

                    var sessionId = Guid.NewGuid().ToString();
                    hiddenReference.Value = sessionId;
                    
                    var page = CmsService.Instance.GetItem<Entity>(itemId);
                    var fieldName = "accordionContent";
                
                    // Parse the accordion data into session

                    var accordionData = AccordionData.Parse(itemId.ToString(), page.EntityName, page.Icon, page[fieldName]);

                    //#region Test Data

                    //accordionData.AddItem(accordionData.Id, "Yo Wazzup", "Im in content", "");

                    //#endregion

                    HttpContext.Current.Session["AccordionData_" + sessionId] = accordionData;
                    treeview.ProviderReferenceId = sessionId;
                    treeview.Provider = typeof(AccordionEditorProvider).GetShortAssemblyName();
                    treeview.ItemId = itemId.ToString();

                    modulePicker.ProviderReferenceId = itemId.ToString();
                    modulePicker.Provider = typeof (LinqIt.UmbracoCustomFieldTypes.UmbracoTreeModuleProvider).GetShortAssemblyName();
                    modulePicker.ItemId = itemId.ToString();
                }
            }
        }

        [AjaxMethod(AjaxType.Sync)]
        public static string AddAccordionChild(string itemId, string referenceId)
        {
            var data = GetAccordionData(referenceId);
            return data.AddItem(itemId, "New Accordion Item", "", "").Id;
        }

        [AjaxMethod(AjaxType.Sync)]
        public static JSONObject AddAccordionSibling(string itemId, string referenceId)
        {
            var data = GetAccordionData(referenceId);
            var item = data.GetItem(itemId);
            var parent = item.Parent;
            var newItem = data.AddItem(parent.Id, "New Accordion Item", "", "");
            var result = new JSONObject();
            result.AddValue("addedId", newItem.Id);
            result.AddValue("parentId", parent.Id);
            return result;
        }




        [AjaxMethod(AjaxType.Sync)]
        public static string UpdateValues(string referenceId, string itemId, string headline, string content, string moduleId)
        {
            var data = GetAccordionData(referenceId);
            var item = data.GetItem(itemId);
            item.Headline = headline;
            item.Text = headline;
            item.Content = content;
            item.ModuleId = moduleId;
            return data.ToJSON().ToString();
        }

        [AjaxMethod(AjaxType.Sync)]
        public static JSONObject RemoveAccordionItem(string referenceId, string itemId)
        {
            var data = GetAccordionData(referenceId);
            var item = data.GetItem(itemId);
            var parentId = item.Parent.Id;
            data.RemoveItem(item);
            var response = new JSONObject();
            response.AddValue("parentId", parentId);
            response.AddValue("updatedValue", data.ToJSON().ToString());
            return response;
        }

        [AjaxMethod(AjaxType.Sync)]
        public static JSONObject MoveAccordionItem(string referenceId, string itemId, int step)
        {
            var data = GetAccordionData(referenceId);
            var item = data.GetItem(itemId);
            var parent = item.Parent;
            var index = parent.Items.IndexOf(item) + step;
            parent.Items.Remove(item);
            parent.Items.Insert(index, item);

            var response = new JSONObject();
            response.AddValue("parentId", parent.Id);
            response.AddValue("updatedValue", data.ToJSON().ToString());
            return response;
        }

        [AjaxMethod(AjaxType.Sync)]
        public static JSONObject GetValues(string referenceId, string itemId)
        {
            var data = GetAccordionData(referenceId);
            var item = data.GetItem(itemId);
            
            JSONObject result = new JSONObject();
            result.AddValue("headline", item.Headline);
            result.AddValue("content", item.Content);
            
            result.AddValue("isRoot", item.Id == data.Id);

            if (item.Id == data.Id)
            {
                result.AddValue("canMoveUp", false);
                result.AddValue("canMoveDown", false);
            }
            else
            {
                var parent = item.Parent;
                var index = item.Parent.Items.IndexOf(item);
                result.AddValue("canMoveUp", index > 0);
                result.AddValue("canMoveDown", index < parent.Items.Count-1);
            }

            if (!string.IsNullOrEmpty(item.ModuleId))
            {
                var module = CmsService.Instance.GetItem<Entity>(new Id(item.ModuleId));
                if (module != null)
                {
                    result.AddValue("moduleId", item.ModuleId);
                    result.AddValue("moduleName", module.EntityName);
                }
            }
            return result;
        }

        private static AccordionData GetAccordionData(string referenceId)
        {
            return (AccordionData) HttpContext.Current.Session["AccordionData_" + referenceId];
        }
    }
}