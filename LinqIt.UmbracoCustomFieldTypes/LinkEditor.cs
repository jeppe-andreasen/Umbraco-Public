using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Components;
using LinqIt.Components.Data;
using umbraco.cms.businesslogic.datatype;

namespace LinqIt.UmbracoCustomFieldTypes
{
    public class LinkEditor : umbraco.cms.businesslogic.datatype.AbstractDataEditor
    {
        private readonly LinqItLinkEditor _control = new LinqItLinkEditor();

        [DataEditorSetting("Provider", description = "The assembly qualified name of the LinkEditorProvider type which provides options", defaultValue = "")]
        public string Provider { get; set; }

        // Set ID, needs to be unique
        public override Guid Id
        {
            get
            {
                return new Guid("5A6EC9DF-42B6-4390-98B2-D878D7E06215");
            }
        }

        //Set name, (is what appears in data editor dropdown)
        public override string DataTypeName
        {
            get
            {
                return "GoBasic LinkEditor";
            }
        }

        public LinkEditor()
        {
            base.RenderControl = _control;
            _control.Init +=OnEditorInitialized;
            DataEditorControl.OnSave += OnSave;
        }

        void OnSave(EventArgs e)
        {
            base.Data.Value = _control.Value;
        }

        void  OnEditorInitialized(object sender, EventArgs e)
        {
            _control.Provider = Provider;
            _control.ReferenceId =  _control.Page.Request.QueryString["id"]; 
            if (base.Data.Value != null)
                _control.Value = base.Data.Value.ToString();
        }
    }
}
