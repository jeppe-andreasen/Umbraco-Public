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
using LinqIt.Components.Data;
using LinqIt.UmbracoCustomFieldTypes;
using LinqIt.Utils.Extensions;

namespace UmbracoPublic.WebSite.handlers.ImageGallery
{
    public partial class Handler : System.Web.UI.Page
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
                    const string fieldName = "imageGalleryContent";

                    // Parse the ImageGallery data into session
                    var imageGalleryData = ImageGalleryData.Parse(page[fieldName]);

                    //#region Test Data

                    //ImageGalleryData.AddItem(ImageGalleryData.Id, "Yo Wazzup", "Im in content", "");

                    //#endregion

                    HttpContext.Current.Session["ImageGalleryData_" + sessionId] = imageGalleryData;
                    treeview.ProviderReferenceId = sessionId;
                    treeview.Provider = typeof(ImageGalleryProvider).GetShortAssemblyName();
                    treeview.ItemId = itemId.ToString();

                    imagePicker.ProviderReferenceId = itemId.ToString();
                    imagePicker.Provider = typeof(UmbracoImageTreeProvider).GetShortAssemblyName();
                    imagePicker.ItemId = itemId.ToString();
                }
            }
        }

        [AjaxMethod(AjaxType.Sync)]
        public static JSONObject AddImageGalleryChild(string itemId, string referenceId)
        {
            var item = new ImageGalleryItem("New Image", Id.Null, "", "");
            var data = GetImageGalleryData(referenceId);
            var before = data.Items.Where(i => i.Id == itemId).FirstOrDefault();
            if (before == null)
                data.Items.Add(item);
            else
                data.Items.Insert(data.Items.IndexOf(before)+1, item);

            var result = new JSONObject();
            result.AddValue("rootId", HttpContext.Current.Request.QueryString["itemId"]);
            result.AddValue("addedId", item.Id.ToString());
            return result;
        }
       
        //[AjaxMethod(AjaxType.Sync)]
        //public static string UpdateValues(string referenceId, string itemId, string headline, string content, string imageId)
        //{
        //    var data = GetImageGalleryData(referenceId);
        //    var item = data.Items.Where(i => i.Id == itemId).FirstOrDefault();
        //    item.Headline = headline;
        //    item.Text = headline;
        //    item.Content = content;
        //    item.ImageId = imageId;
        //    return data.ToJSON().ToString();
        //}

        [AjaxMethod(AjaxType.Sync)]
        public static JSONObject SetItemValue(string referenceId, string itemId, JSONObject value)
        {
            var result = new JSONObject();
            var data = GetImageGalleryData(referenceId);
            var item = data.Items.Where(i => i.Id == itemId).FirstOrDefault();
            foreach (var key in value.Keys)
            {
                switch (key)
                {
                    case "name":
                        item.Text = (string) value[key];
                        break;
                    case "headline":
                        item.Headline = (string) value[key];
                        break;
                    case "content":
                        item.Content = (string) value[key];
                        break;
                    case "imageId":
                        item.ImageId = (string) value[key];
                        if (!string.IsNullOrEmpty(item.ImageId))
                        {
                            var media = new global::umbraco.cms.businesslogic.media.Media(Convert.ToInt32(item.ImageId));
                            {
                                result.AddValue("imageId", item.ImageId);
                                result.AddValue("imageName", media.Text);
                                result.AddValue("imageUrl", (string)media.getProperty("umbracoFile").Value);
                            }
                        }
                        break;
                }
            }
            result.AddValue("value", data.ToJSON().ToString());
            return result;
        }

        [AjaxMethod(AjaxType.Sync)]
        public static JSONObject RemoveImageGalleryItem(string referenceId, string itemId)
        {
            var data = GetImageGalleryData(referenceId);
            var item = data.Items.Where(i => i.Id == itemId).FirstOrDefault();
            data.Items.Remove(item);
            var response = new JSONObject();
            response.AddValue("updatedValue", data.ToJSON().ToString());
            response.AddValue("parentId", HttpContext.Current.Request.QueryString["itemId"]);
            return response;
        }

        [AjaxMethod(AjaxType.Sync)]
        public static JSONObject MoveImageGalleryItem(string referenceId, string itemId, int step)
        {
            var data = GetImageGalleryData(referenceId);
            var item = data.Items.Where(i => i.Id == itemId).FirstOrDefault();
            if (item != null)
            {
                var index = data.Items.IndexOf(item) + step;
                data.Items.Remove(item);
                data.Items.Insert(index, item);                        
            }
            var response = new JSONObject();
            response.AddValue("updatedValue", data.ToJSON().ToString());
            response.AddValue("parentId", HttpContext.Current.Request.QueryString["itemId"]);
            return response;
        }

        [AjaxMethod(AjaxType.Sync)]
        public static JSONObject GetValues(string referenceId, string itemId)
        {
            var data = GetImageGalleryData(referenceId);

            var result = new JSONObject();

            var item = data.Items.Where(i => i.Id == itemId).FirstOrDefault();
            bool isRoot = item== null || item.Id == HttpContext.Current.Request.QueryString["itemid"];

            result.AddValue("isRoot", isRoot);

            if (isRoot)
            {
                result.AddValue("canMoveUp", false);
                result.AddValue("canMoveDown", false);
                result.AddValue("headline", "");
                result.AddValue("content", "");
            }
            else
            {
                var index = data.Items.IndexOf(item);
                result.AddValue("canMoveUp", index > 0);
                result.AddValue("canMoveDown", index < data.Items.Count - 1);
                result.AddValue("name", item.Text);
                result.AddValue("headline", item.Headline);
                result.AddValue("content", item.Content);
                if (!string.IsNullOrEmpty(item.ImageId))
                {
                    var media = new global::umbraco.cms.businesslogic.media.Media(Convert.ToInt32(item.ImageId));
                    {
                        result.AddValue("imageId", item.ImageId);
                        result.AddValue("imageName", media.Text);
                        result.AddValue("imageUrl", (string)media.getProperty("umbracoFile").Value);
                    }
                }
            }
            return result;
        }

        private static ImageGalleryData GetImageGalleryData(string referenceId)
        {
            return (ImageGalleryData)HttpContext.Current.Session["ImageGalleryData_" + referenceId];
        }
    }
}