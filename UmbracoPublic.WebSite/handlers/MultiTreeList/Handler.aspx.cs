using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Ajax;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using LinqIt.Components;
using LinqIt.Components.Data;

namespace UmbracoPublic.WebSite.handlers.MultiTreeList
{
    public partial class Handler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxUtil.RegisterPageMethods(this);

            if (!string.IsNullOrEmpty(Request.QueryString["frame"]))
            {
                ClientScript.RegisterStartupScript(Page.GetType(), "editorInitialization", @"
                var gridEditorFrame = $('#" + Request.QueryString["frame"] + @"', window.parent.document);
                gridEditorFrame.parent().css('clear','both');
                ", true);
            }

            editor.Attributes.Add("hiddenId", Request.QueryString["hiddenId"]);
        }

        protected override void CreateChildControls()
        {
            var provider = GetProvider();

            using (CmsContext.Editing)
            {
                var item = CmsService.Instance.GetItem<Entity>(new Id(Request.QueryString["itemId"]));
                var fieldName = Request.QueryString["fieldName"];
                var value = new IdList(item[fieldName]);
                var destinationNodes = value.Select(id => provider.GetNode(id.ToString())).Where(node => node != null).ToArray();

                var treeview = new LinqItTreeView();
                treeview.Provider = Request.QueryString["provider"];
                treeview.ProviderReferenceId = Request.QueryString["itemId"];

                var placeholder = new PlaceHolder();
                placeholder.Controls.Add(new LiteralControl("<div class=\"srcList \">"));
                placeholder.Controls.Add(treeview);
                placeholder.Controls.Add(new LiteralControl("</div>"));

                multiListControl.Initialize(placeholder, destinationNodes);
            }
            base.CreateChildControls();
        }

        public static TreeNodeProvider GetProvider()
        {
            var queryString = HttpContext.Current.Request.QueryString;
            return ProviderHelper.GetProvider<TreeNodeProvider>(queryString["provider"], queryString["itemId"]);
        }
    }
}