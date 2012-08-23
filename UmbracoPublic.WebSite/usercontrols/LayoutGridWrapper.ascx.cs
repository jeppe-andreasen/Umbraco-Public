using System;
using System.Collections.Specialized;
using System.Linq;
using LinqIt.Components;
using LinqIt.Utils.Extensions;
using umbraco.cms.businesslogic.datatype;
using umbraco.editorControls.userControlGrapper;

namespace UmbracoPublic.WebSite.usercontrols
{
    public partial class LayoutGridWrapper : System.Web.UI.UserControl, IUsercontrolDataEditor
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnPreRender(EventArgs e)
        {
            var parameters = new NameValueCollection();
            parameters.Add("itemId", Request.QueryString["id"]);
            parameters.Add("frame", gridEditorFrame.ClientID);
            parameters.Add("hiddenId", hiddenValue.ClientID);
            parameters.Add("fieldName", FieldName);
            parameters.Add("layoutClass", LayoutClass);

            if (ValidateLayout())
                gridEditorFrame.Attributes.Add("src", "/handlers/GridEditorHandler.aspx?" + parameters.ToUrlParameterList());
            else
                gridEditorFrame.Attributes.Add("src", "/handlers/FixGridLayoutHandler.aspx?" + parameters.ToUrlParameterList());
            
            base.OnPreRender(e);
        }

        private bool ValidateLayout()
        {
            var provider = new LinqIt.UmbracoCustomFieldTypes.UmbracoTreeModuleProvider(Request.QueryString["id"]);
            var placeholderData = provider.GetPlaceholderData();

            GridLayout layout;
            if (!string.IsNullOrEmpty(LayoutClass))
            {
                var layoutClassType = Type.GetType(LayoutClass);
                if (layoutClassType == null)
                    throw new ApplicationException("Invalid Layout Class Type : " + LayoutClass);
                var layoutClass = Activator.CreateInstance(layoutClassType) as IGridModuleControl;
                if (layoutClass == null)
                    throw new ApplicationException("The layout class must implement IGridModuleControl");
                layout = layoutClass.GetGridLayout();
            }
            else
                layout = provider.GetLayout();

            var cells = layout.GetPlaceholderCells();
            foreach (var placeholder in placeholderData.Keys.Where(k => placeholderData[k].Items.Any()))
            {
                var cell = cells.Where(c => string.Compare(c.Key, placeholder, true) == 0).FirstOrDefault();
                if (cell == null)
                {
                    // A placeholder does not exist in the current layout with the given key
                    return false;
                }
                if (placeholderData[placeholder].Items.Where(i => i.ColumnSpan > cell.ColumnSpan).Any())
                {
                    // A module exists which is too large to fit in the current cell
                    return false;
                }
            }
            return true;
        }

        [DataEditorSetting("FieldName")]
        public string FieldName { get; set; }

        [DataEditorSetting("LayoutClass")]
        public string LayoutClass { get; set; }

        public object value
        {
            get { return hiddenValue.Value; }
            set { hiddenValue.Value = value.ToString(); }
        }
    }
}