using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Components;
using umbraco.cms.businesslogic.datatype;

namespace LinqIt.UmbracoCustomFieldTypes
{
    public class LinkListEditor: umbraco.cms.businesslogic.datatype.AbstractDataEditor
    {
        private readonly LinqItLinkListEditor _control = new LinqItLinkListEditor();

        [DataEditorSetting("Provider", description = "The assembly qualified name of the LinkEditorProvider type which provides options", defaultValue = "")]
        public string Provider { get; set; }

        // Set ID, needs to be unique
        public override Guid Id
        {
            get
            {
                return new Guid("D210FFCE-449B-4EF2-AB75-F96FA559778F");
            }
        }

        //Set name, (is what appears in data editor dropdown)
        public override string DataTypeName
        {
            get
            {
                return "GoBasic Link-List Editor";
            }
        }

        public LinkListEditor()
        {
            base.RenderControl = _control;
            _control.Init += OnEditorInitialized;
            DataEditorControl.OnSave += OnSave;
        }

        void OnSave(EventArgs e)
        {
            base.Data.Value = _control.Value;
        }

        void OnEditorInitialized(object sender, EventArgs e)
        {
            _control.Provider = Provider;
            _control.ReferenceId =  _control.Page.Request.QueryString["id"]; 
            if (base.Data.Value != null)
                _control.Value = base.Data.Value.ToString();
        }
    }
}
