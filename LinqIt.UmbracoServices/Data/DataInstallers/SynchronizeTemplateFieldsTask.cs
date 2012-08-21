using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Xml;
using umbraco.cms.businesslogic.datatype;
using umbraco.cms.businesslogic.propertytype;
using umbraco.cms.businesslogic.web;

namespace LinqIt.UmbracoServices.Data.DataInstallers
{
    public class SynchronizeTemplateFieldsTask : SynchronizationTask
    {
        private readonly UmbracoDataContext _context;
        private readonly XmlElement[] _input;
        private readonly DocumentType _container;
        private readonly Dictionary<string, DataTypeDefinition> _datatypes;
        private readonly Dictionary<string, int> _tabIds;

        public SynchronizeTemplateFieldsTask(UmbracoDataContext context, XmlElement[] input, DocumentType container)
        {
            _context = context;
            _input = input;
            _container = container;
            _datatypes = DataTypeDefinition.GetAll().ToDictionary(dt => dt.Text);
            
            _tabIds = _context.cmsTabs.ToDictionary(t => DataHelper.GetPath(_context, t), t => t.id);
        }

        protected override void AddMissingItems(StringBuilder log)
        {
            var existingFields = _container.PropertyTypes.ToDictionary(t => t.Alias);
            foreach (var element in _input)
            {
                var alias = element.GetAttribute("alias");
                if (!existingFields.ContainsKey(alias))
                {
                    if (!_datatypes.ContainsKey(element.GetAttribute("datadef")))  
                        throw new ApplicationException("Unknown field datatype: " + element.GetAttribute("datadef"));
                    var dt = _datatypes[element.GetAttribute("datadef")];
                   _container.AddPropertyType(dt, alias, element.GetAttribute("name"));
                }
            }
        }

        protected override void UpdateItems(StringBuilder log)
        {
            var existingFields = _container.PropertyTypes.ToDictionary(t => t.Alias);
            foreach (var element in _input)
            {
                var requiresSave = false;
                var alias = element.GetAttribute("alias");
                var property = existingFields[alias];

                #region Update Tab
                if (element.HasAttribute("tab"))
                {
                    if (!_tabIds.ContainsKey(element.GetAttribute("tab")))
                        throw new ApplicationException("Unknown tab: " + element.GetAttribute("tab"));

                    var tabId = _tabIds[element.GetAttribute("tab")];
                    if (property.TabId != tabId)
                    {
                        property.TabId = tabId;
                        requiresSave = true;
                    }
                }
                else if (property.TabId != 0)
                {
                    property.TabId = 0;
                    requiresSave = true;
                }
                #endregion

                #region Update DataTypeDefinition

                if (!_datatypes.ContainsKey(element.GetAttribute("datadef")))
                    throw new ApplicationException("Unknown datatype: " + element.GetAttribute("datadef"));

                var dt = _datatypes[element.GetAttribute("datadef")];
                if (property.DataTypeDefinition.Id != dt.Id)
                {
                    property.DataTypeDefinition = dt;
                    requiresSave = true;
                }

                #endregion

                #region Update Name

                var name = element.GetAttribute("name");
                if (property.Name != name)
                {
                    property.Name = name;
                    requiresSave = true;
                }

                #endregion

                #region Update Sortorder

                var sortOrder = Convert.ToInt32(element.GetAttribute("sortOrder"));
                if (property.SortOrder != sortOrder)
                {
                    property.SortOrder = sortOrder;
                    requiresSave = true;
                }

                #endregion

                #region Update Mandatory

                var mandatory = element.GetAttribute("mandatory") == "true";
                if (property.Mandatory != mandatory)
                {
                    property.Mandatory = mandatory;
                    requiresSave = true;
                }

                #endregion

                if (requiresSave)
                    property.Save();

            }
        }

        protected override void DeleteOldItems(StringBuilder log)
        {
            var existingFields = _container.PropertyTypes.ToDictionary(t => t.Alias);
            foreach (var element in _input)
            {
                var alias = element.GetAttribute("alias");
                existingFields.Remove(alias);
            }
            foreach (var property in existingFields.Values)
                property.delete();
        }

    }
}
