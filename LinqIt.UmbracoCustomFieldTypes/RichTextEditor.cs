using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using LinqIt.Parsing.Html;
using LinqIt.UmbracoCustomFieldTypes.Components;
using LinqIt.UmbracoCustomFieldTypes.Utilities;
using LinqIt.Utils.Extensions;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.datatype;
using UmbracoPublic.Logic.Entities;
using helper = umbraco.helper;

namespace LinqIt.UmbracoCustomFieldTypes
{
    public class RichTextEditor : umbraco.cms.businesslogic.datatype.AbstractDataEditor
    {
        private readonly TinyMCEditor _control = new TinyMCEditor();

        // Set ID, needs to be unique
        public override Guid Id
        {
            get
            {
                return new Guid("2BE205EC-4B79-425D-8D81-969BA4B91DF6");
            }
        }

        //Set name, (is what appears in data editor dropdown)
        public override string DataTypeName
        {
            get
            {
                return "GoBasic Richtext Editor";
            }
        }

        public RichTextEditor()
        {
            base.RenderControl = _control;
            _control.Init += OnControlInitialized;
            DataEditorControl.OnSave += OnControlSaved;
        }

        void OnControlInitialized(object sender, EventArgs e)
        {
            using (var context = CmsContext.Editing)
            {
                var currentItem = CmsService.Instance.GetItem<Entity>(new Id(HttpContext.Current.Request.QueryString["id"]));
                context.Path = currentItem.Path;

                _control.Value = ConvertFromEditor(base.Data.Value != null ? base.Data.Value.ToString() : string.Empty);
                _control.StyleDefinitionSheet = StyleDefinitionSheet;
                _control.StyleSheets = Theme.Current.TinyMCEStylesheets.ToArray();
            }
        }

        void OnControlSaved(EventArgs e)
        {
            base.Data.Value = ConvertToEditor(_control.Value ?? string.Empty);
        }

        [DataEditorSetting("Style definition sheet", description = "The name of the stylesheet containing the style options", defaultValue = "")]
        public string StyleDefinitionSheet { get; set; }

        private static string ConvertToEditor(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            return UmbracoRichTextHelper.ConvertHtmlToMacros(value);
        }

        private string ConvertFromEditor(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            var pageId = ((DefaultData)this.Data).NodeId;
            var versionId = ((DefaultData)this.Data).Version;
            return UmbracoRichTextHelper.ConvertMacrosToHtml(value, pageId, versionId);
        }
    }

}