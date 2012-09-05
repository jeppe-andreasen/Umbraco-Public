using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using LinqIt.Utils.Extensions;
using umbraco.cms.businesslogic.datatype;
using umbraco.editorControls.userControlGrapper;

namespace LinqIt.UmbracoCustomFieldTypes
{
    public abstract class BaseUCEditorWrapper : System.Web.UI.UserControl, IUsercontrolDataEditor
    {
        private HiddenField _hiddenField;
        private HtmlGenericControl _iframe;

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            _hiddenField = new HiddenField();
            _hiddenField.ID = "hiddenValue";
            Controls.Add(_hiddenField);

            _iframe = new HtmlGenericControl("iframe");
            _iframe.ID = "iframe";
            _iframe.Attributes.Add("frameBorder", "0");
            _iframe.Attributes.Add("style", "width:100%");
            _iframe.Attributes.Add("height", Height.ToString());
            _iframe.Attributes.Add("scrolling", "no");
            Controls.Add(_iframe);

            var parameters = new NameValueCollection();
            
            AddParameters(parameters);
           
            _iframe.Attributes.Add("src", HandlerUrl + "?" + parameters.ToUrlParameterList());
        }

        protected abstract int Height { get; }

        protected abstract string HandlerUrl { get; }

        [DataEditorSetting("FieldName")]
        public string FieldName { get; set; }

        protected virtual void AddParameters(NameValueCollection parameters)
        {
            parameters.Add("itemId", Request.QueryString["id"]);
            parameters.Add("frame", _iframe.ClientID);
            parameters.Add("hiddenId", _hiddenField.ClientID);
            parameters.Add("fieldName", FieldName);
        }

        public object value
        {
            get
            {
                EnsureChildControls();
                return _hiddenField.Value;
            }
            set
            {
                EnsureChildControls();
                _hiddenField.Value = value.ToString();
            }
        }
    }
}
