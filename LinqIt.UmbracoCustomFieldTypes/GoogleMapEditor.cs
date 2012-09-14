using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Components;
using umbraco.cms.businesslogic.datatype;

namespace LinqIt.UmbracoCustomFieldTypes
{
    public class GoogleMapEditor: umbraco.cms.businesslogic.datatype.AbstractDataEditor
    {
        private readonly LinqItGoogleMapEditor _control = new LinqItGoogleMapEditor();

        // Set ID, needs to be unique
        public override Guid Id
        {
            get
            {
                return new Guid("CE9F0294-5D40-4B30-B878-C3056E055D3F");
            }
        }

        //Set name, (is what appears in data editor dropdown)
        public override string DataTypeName
        {
            get
            {
                return "GoBasic GoogleMapEditor";
            }
        }

        public GoogleMapEditor()
        {
            base.RenderControl = _control;
            _control.Init += OnEditorInitialized;
            DataEditorControl.OnSave += OnSave;
        }

        void OnSave(EventArgs e)
        {
            base.Data.Value = _control.Value;
        }

        void  OnEditorInitialized(object sender, EventArgs e)
        {
            if (base.Data.Value != null)
                _control.Value = base.Data.Value.ToString();
        }
    }
}
