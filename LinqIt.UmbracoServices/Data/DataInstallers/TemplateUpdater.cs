using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using LinqIt.Utils.Extensions;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.datatype;
using umbraco.cms.businesslogic.propertytype;
using umbraco.cms.businesslogic.web;

namespace LinqIt.UmbracoServices.Data.DataInstallers
{
    public class TemplateUpdater
    {
        private DocumentType _documentType;
        private readonly string _connectionString;
        private readonly XmlElement _element;
        private bool _requiresSave;

        public TemplateUpdater(DocumentType documentType, string connectionString, XmlElement element)
        {
            _documentType = documentType;
            _connectionString = connectionString;
            _element = element;
            _requiresSave = false;
        }

        public void UpdateTemplate(StringBuilder log, Dictionary<string, DocumentType> documentTypeLookup)
        {
            log.AppendLine("");
            log.AppendLine("** Updating document type " + _documentType.Text + " **");

            UpdateDocTypeIconAndThumbnail(log);

            UpdateName(log);

            if (_requiresSave)
                PersistDocumentType();
        }



        private void UpdateName(StringBuilder log)
        {
            var name = _element.GetAttribute("displayName");
            if (_documentType.Text != name)
            {
                var oldName = _documentType.Text;
                _documentType.Text = name;
                _requiresSave = true;
                log.AppendLine("SET DT Displayname: " + oldName + " -> " + name);
            }
        }

        private void UpdateDocTypeIconAndThumbnail(StringBuilder log)
        {
            var changesMade = false;
            var dataContext = new UmbracoDataContext(_connectionString);
            var contentType = dataContext.cmsContentTypes.Where(t => t.nodeId == _documentType.Id).FirstOrDefault();
            var icon = _element.GetAttribute("icon");
            if (contentType.icon != icon)
            {
                contentType.icon = icon;
                changesMade = true;
                log.AppendLine("SET DT Icon: " + _documentType.Text + " -> " + icon);
            }
            var thumbnail = _element.GetAttribute("thumbnail");
            if (thumbnail != contentType.thumbnail)
            {
                contentType.thumbnail = thumbnail;
                changesMade = true;
                log.AppendLine("SET DT Thumbnail: " + _documentType.Text + " -> " + thumbnail);
            }

            if (changesMade)
            {
                dataContext.SubmitChanges();
                RefreshDocumentType();
            }
        }

        private void PersistDocumentType()
        {
            _documentType.Save();
            _requiresSave = false;
            RefreshDocumentType();
        }

        private void RefreshDocumentType()
        {
            ContentType.RemoveFromDataTypeCache(_documentType.Alias);
            _documentType = DocumentType.GetByAlias(_documentType.Alias);
        }

        internal void UpdateChildStrucure(StringBuilder log, Dictionary<string, DocumentType> documentTypeLookup)
        {
            var oldHash = GetHash(_documentType.AllowedChildContentTypeIDs);
            var paths = _element.SelectNodes("allowedChildren/add").Cast<XmlElement>().Select(n => n.InnerText).ToArray();
            var invalidKeys = paths.Where(p => !documentTypeLookup.ContainsKey(p)).ToArray();
            if (invalidKeys.Any())
                throw new ApplicationException("Invalid child structure keys: " + invalidKeys.ToSeparatedString(", "));

            var newElements = paths.Select(p => documentTypeLookup[p].Id).ToArray();
            var newHash = GetHash(newElements);
            if (newHash != oldHash)
            {
                _documentType.AllowedChildContentTypeIDs = newElements;
                _requiresSave = true;
                log.AppendLine("SET DT Structure: " + _documentType.Text + " -> " + oldHash + " => " + newHash);
            }
            if (_requiresSave)
                PersistDocumentType();
        }

        private static string GetHash(IEnumerable<int> items)
        {
            return items.OrderBy(i => i).ToSeparatedString(",");
        }

        internal void UpdateTabs(StringBuilder log, Dictionary<string, DocumentType> documentTypeLookup)
        {
            var existingTabs = _documentType.getVirtualTabs.Where(t => t.ContentType == _documentType.Id).ToDictionary(t => t.Caption);
            foreach (var tabElement in _element.SelectNodes("tabs/tab").Cast<XmlElement>())
            {
                var name = tabElement.GetAttribute("name");
                var sortOrder = Convert.ToInt32(tabElement.GetAttribute("sortOrder"));
                if (!existingTabs.ContainsKey(name))
                {
                    int tabId = _documentType.AddVirtualTab(name);
                    _documentType.SetTabSortOrder(tabId, sortOrder);
                    log.AppendLine("ADD tab: " + _documentType.Text + " -> " + name);
                }
                else
                {
                    var tab = existingTabs[name];
                    if (tab.SortOrder != sortOrder)
                    {
                        _documentType.SetTabSortOrder(tab.Id, sortOrder);
                        log.AppendLine("SET tab sortorder: " + _documentType.Text + "." + name + " -> " + sortOrder);
                    }
                    existingTabs.Remove(name);
                }
            }
            foreach (var tab in existingTabs.Values)
            {
                _documentType.DeleteVirtualTab(tab.Id);
                log.AppendLine("REMOVE Tab: " + _documentType.Text + " -> " + tab.Caption);
            }
        }

        internal void UpdateProperties(StringBuilder log, Dictionary<string, DocumentType> documentTypeLookup)
        {
            var existingProperties = _documentType.PropertyTypes.ToDictionary(t => t.Alias);
            var dataDefinitions = DataTypeDefinition.GetAll().ToDictionary(t => t.Text);

            var dataContext = new UmbracoDataContext(_connectionString);
            var tabs = dataContext.cmsTabs.ToDictionary(t => DataHelper.GetPath(dataContext, t));

            foreach (XmlElement element in _element.SelectNodes("properties/add"))
            {
                var alias = element.GetAttribute("alias");
                var name = element.GetAttribute("name");
                var dataTypeName = element.GetAttribute("datadef");
                if (!dataDefinitions.ContainsKey(dataTypeName))
                    throw new ApplicationException("Unknown data type: " + dataTypeName);
                var dataType = dataDefinitions[dataTypeName];
                
                var property = !existingProperties.ContainsKey(alias) ? _documentType.AddPropertyType(dataType, alias, name) : existingProperties[alias];
                if (property.Name != name)
                {
                    var oldName = property.Name;
                    property.Name = name;
                    _requiresSave = true;
                    log.AppendLine("SET name on property: " + _documentType.Text + " -> " + oldName + " => " + name);
                }
                
                var mandatory = element.GetAttribute("mandatory") == "true";
                if (property.Mandatory != mandatory)
                {
                    property.Mandatory = mandatory;
                    _requiresSave = true;
                    log.AppendLine("SET mandatory on property: " + _documentType.Text + "." + property.Name + " -> " + (mandatory ? "true" : "false"));
                }

                var sortOrder = Convert.ToInt32(element.GetAttribute("sortOrder"));
                if (property.SortOrder != sortOrder)
                {
                    property.SortOrder = sortOrder;
                    _requiresSave = true;
                    log.AppendLine("SET sortorder on property: " + _documentType.Text + "." + property.Name + " -> " + sortOrder);
                }

                if (_requiresSave)
                    property.Save();

                if (element.HasAttribute("tab"))
                {
                    var tabPath = element.GetAttribute("tab");
                    if (!tabs.ContainsKey(tabPath))
                        throw new ApplicationException("Unknown tab: " + tabPath);
                    var tab = tabs[tabPath];
                    if (property.TabId != tab.id)
                    {
                        _documentType.SetTabOnPropertyType(property, tab.id);
                        log.AppendLine("SET tab on property: " + _documentType.Text + "." + property.Name + " -> " + tab.text);    
                    }
                }
                else if (property.TabId != 0)
                {
                    _documentType.removePropertyTypeFromTab(property);
                    log.AppendLine("Remove tab on property: " + _documentType.Text + "." + property.Name);
                }
            }
        }
    }
}
