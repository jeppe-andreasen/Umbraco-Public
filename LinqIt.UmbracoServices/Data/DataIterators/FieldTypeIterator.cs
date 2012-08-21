using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data.DataIterators;
using LinqIt.Utils.Extensions;
using umbraco.cms.businesslogic.datatype;
using umbraco.cms.businesslogic.web;

namespace LinqIt.UmbracoServices.Data.DataIterators
{
    public class FieldTypeIterator : DataIterator
    {
        private readonly DataTypeDefinition[] _definitions;
        private readonly UmbracoDataContext _dataContext;
        private int _index = -1;
        private string[] _invalidPaths;

        public FieldTypeIterator(UmbracoDataContext dataContext, string[] invalidPaths)
        {
            _definitions = DataTypeDefinition.GetAll();
            _dataContext = dataContext;
            _invalidPaths = invalidPaths;
        }

        protected override string ItemType
        {
            get { return "dataTypeDefinitions"; }
        }

        protected override bool ReadNext()
        {
            if (_index < _definitions.Length - 1)
            {
                _index++; return true;
            }
            return false;
        }

        protected override void RenderCurrent(System.Xml.XmlWriter writer)
        {
            var definition = _definitions[_index];
            if (_invalidPaths.Contains("fieldtypes/" + definition.Text.ToLower()))
                return;

            writer.WriteStartElement("datatypeDefinition");
            writer.WriteAttributeString("name", definition.Text);
            writer.WriteAttributeString("dataType", definition.DataType.DataTypeName);
            writer.WriteAttributeString("dataTypeClass", definition.DataType.GetType().GetShortAssemblyName());
            foreach (var prevalue in _dataContext.cmsDataTypePreValues.Where(p => p.datatypeNodeId == definition.Id))
            {
                writer.WriteStartElement("preValue");
                if (!string.IsNullOrEmpty(prevalue.alias))
                    writer.WriteAttributeString("alias", prevalue.alias);
                writer.WriteAttributeString("value", prevalue.value);
                writer.WriteAttributeString("sortOrder", prevalue.sortorder.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }


    }
}
