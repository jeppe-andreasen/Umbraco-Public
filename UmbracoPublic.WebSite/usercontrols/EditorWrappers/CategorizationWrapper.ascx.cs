using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using LinqIt.Utils.Extensions;
using umbraco.cms.businesslogic.datatype;
using umbraco.editorControls.userControlGrapper;
using UmbracoPublic.Logic.Entities;

namespace UmbracoPublic.WebSite.usercontrols.EditorWrappers
{
    public partial class CategorizationWrapper : System.Web.UI.UserControl, IUsercontrolDataEditor
    {
        private CategorizationFolder _categorizations;
        private readonly Dictionary<Id, Control> _editorControls = new Dictionary<Id, Control>();

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void CreateChildControls()
        {
            _categorizations = CategorizationFolder.Get(new Id(Request.QueryString["id"]));
            foreach (var type in _categorizations.Types)
            {
                this.Controls.Add(new LiteralControl("<p>"));
                var label = new Label();
                label.Text = type.EntityName;
                this.Controls.Add(label);
                this.Controls.Add(new LiteralControl("<br>"));
                if (type.AllowMultipleSelections || string.Compare(AllowMultipleOnAll,"true", true) == 0)
                {
                    var multilist = (MultiListWrapper)LoadControl("~/usercontrols/MultiListWrapper.ascx");
                    multilist.Provider = Provider;
                    multilist.FieldName = FieldName;
                    //multilist.ID = "ctrl_" + n;
                    multilist.RootId = type.Id.ToString();
                    this.Controls.Add(multilist);
                    _editorControls.Add(type.Id, multilist);
                }
                else
                {
                    var ddl = new DropDownList();
                    ddl.AddDefaultItem();
                    foreach (var item in type.Items)
                    {
                        ddl.Items.Add(new ListItem(item.DisplayName, item.Id.ToString()));
                    }
                    //ddl.ID = "ctrl_" + n;
                    this.Controls.Add(ddl);
                    _editorControls.Add(type.Id, ddl);
                }
                this.Controls.Add(new LiteralControl("</p>"));
            }
        }

        [DataEditorSetting("Provider")]
        public string Provider { get; set; }

        [DataEditorSetting("FieldName")]
        public string FieldName { get; set; }

        [DataEditorSetting("AllowMultipleOnAll")]
        public string AllowMultipleOnAll { get; set; }

        public object value
        {
            get
            {
                EnsureChildControls();
                var ids = new IdList();
                foreach (var control in _editorControls.Values)
                {
                    if (control is MultiListWrapper)
                    {
                        var multilist = (MultiListWrapper) control;
                        ids.AddRange(new IdList((string)multilist.value));
                    }
                    else if (control is DropDownList)
                    {
                        var ddl = (DropDownList) control;
                        if (!string.IsNullOrEmpty(ddl.SelectedValue))
                            ids.Add(new Id(ddl.SelectedValue));
                    }
                }
                return ids.ToString();
            }
            set
            {
                EnsureChildControls();
                var values = new IdList((string)value);
                foreach (var typeId in _editorControls.Keys)
                {
                    var type = _categorizations.Types.Where(t => t.Id == typeId).FirstOrDefault();
                    var localValue = new IdList(type.Items.Select(i => i.Id).Where(id => values.Contains(id))).ToString();
                    var control = _editorControls[typeId];
                    if (control is MultiListWrapper)
                    {
                        var multilist = (MultiListWrapper)control;
                        multilist.value = localValue;
                    }
                    else if (control is DropDownList)
                    {
                        var ddl = (DropDownList)control;
                        ddl.SelectByValue(localValue);
                    }
                }
            }
        }
    }
}