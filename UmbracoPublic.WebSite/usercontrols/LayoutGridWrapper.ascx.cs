using System;
using System.Linq;
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
            if (ValidateLayout())
                gridEditorFrame.Attributes.Add("src", "/handlers/GridEditorHandler.aspx?itemId=" + Request.QueryString["id"] + "&frame=" + gridEditorFrame.ClientID + "&hiddenId=" + hiddenValue.ClientID);
            else
                gridEditorFrame.Attributes.Add("src", "/handlers/FixGridLayoutHandler.aspx?itemId=" + Request.QueryString["id"] + "&frame=" + gridEditorFrame.ClientID + "&hiddenId=" + hiddenValue.ClientID);
            base.OnPreRender(e);
        }

        private bool ValidateLayout()
        {
            var itemId = Convert.ToInt32(Request.QueryString["id"]);
            var provider = new LinqIt.UmbracoCustomFieldTypes.UmbracoTreeModuleProvider(Request.QueryString["id"]);
            var placeholderData = provider.GetPlaceholderData();
            var layout = provider.GetLayout();
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

        public object value
        {
            get { return hiddenValue.Value; }
            set { hiddenValue.Value = value.ToString(); }
        }
    }
}