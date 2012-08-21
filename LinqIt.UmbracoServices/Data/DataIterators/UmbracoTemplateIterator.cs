using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using LinqIt.Cms.Data.DataIterators;
using LinqIt.UmbracoServices.Data.DataInstallers;
using umbraco.cms.businesslogic.datatype;

namespace LinqIt.UmbracoServices.Data.DataIterators
{
    class UmbracoTemplateIterator: DataIterator
    {
        private int _index = -1;
        private readonly cmsContentType[] _templates;
        private readonly UmbracoDataContext _dataContext;
        private readonly string[] _invalidPaths;

        public UmbracoTemplateIterator(UmbracoDataContext context, string[] invalidPaths)
        {
            _dataContext = context;
            _invalidPaths = invalidPaths;
            _templates = _dataContext.cmsContentTypes.Where(c => c.masterContentType != null).ToArray();
        }        

        protected override void RenderCurrent(System.Xml.XmlWriter writer)
        {
            var template = _templates[_index]; writer.WriteStartElement("template");
            if (_invalidPaths.Contains("templates/" + template.alias.ToLower()))
                return;

            writer.WriteAttributeString("alias", template.alias);
            writer.WriteAttributeString("displayName", template.umbracoNode.text);
            writer.WriteAttributeString("icon", template.icon);
            writer.WriteAttributeString("thumbnail", template.thumbnail);
            writer.WriteAttributeString("path", DataHelper.GetPath(_dataContext, template));
            if (template.masterContentType != 0)
            {
                var parent = _dataContext.cmsContentTypes.Where(c => c.nodeId == template.masterContentType).FirstOrDefault();
                if (parent != null)
                    writer.WriteAttributeString("parent", DataHelper.GetPath(_dataContext, parent));
            }
            if (!string.IsNullOrEmpty(template.description))
                writer.WriteElementString("description", template.description);
            
            GenerateAllowedChildren(writer, template);

            GenerateTabs(writer, template);

            GenerateFields(writer, template);

            writer.WriteEndElement(); // template        
        }

        private static void GenerateTabs(System.Xml.XmlWriter writer, cmsContentType template)
        {
            var tabs = template.cmsTabs.ToArray();
            if (!tabs.Any())
                return;

            writer.WriteStartElement("tabs");
            foreach (var tab in tabs)
            {
                writer.WriteStartElement("tab");
                writer.WriteAttributeString("name", tab.text);
                writer.WriteAttributeString("sortOrder", tab.sortorder.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            
        }

        private void GenerateFields(XmlWriter writer, cmsContentType template)
        {
            var properties = template.cmsPropertyTypes.ToArray();
            if (!properties.Any())
                return;

            writer.WriteStartElement("properties");
            foreach (var property in properties)
            {
                writer.WriteStartElement("add");
                writer.WriteAttributeString("alias", property.Alias);
                if (property.tabId != null)
                    writer.WriteAttributeString("tab", DataHelper.GetPath(_dataContext, property.cmsTab));
                writer.WriteAttributeString("name", property.Name);
                writer.WriteAttributeString("sortOrder", property.sortOrder.ToString());
                writer.WriteAttributeString("mandatory", property.mandatory ? "true" : "false");

                var def = new DataTypeDefinition(property.dataTypeId);
                writer.WriteAttributeString("datadef", def.Text);
                if (!string.IsNullOrEmpty(property.helpText))
                    writer.WriteElementString("helpText", property.helpText);
                if (!string.IsNullOrEmpty(property.validationRegExp))
                {
                    writer.WriteStartElement("validation");
                    writer.WriteCData(property.validationRegExp);
                    writer.WriteEndElement();
                }
                if (!string.IsNullOrEmpty(property.Description))
                    writer.WriteElementString("description", property.Description);
                writer.WriteEndElement();
            }
            writer.WriteEndElement(); // fields           
        }

        private void GenerateAllowedChildren(System.Xml.XmlWriter writer, cmsContentType template)
        {
            var allowedChildItems = _dataContext.cmsContentTypeAllowedContentTypes.Where(r => r.Id == template.nodeId).ToArray();
            if (allowedChildItems.Any())
            {
                writer.WriteStartElement("allowedChildren");
                foreach (var item in allowedChildItems.Select(i => i.cmsContentType1))
                    writer.WriteElementString("add", DataHelper.GetPath(_dataContext, item));
                writer.WriteEndElement();
            }
        }

        protected override bool ReadNext()
        {
            if (_index < _templates.Length - 1)
            {
                _index++; return true;
            } 
            return false;
        }

        protected override string ItemType { get { return "templates"; } }
    }
}