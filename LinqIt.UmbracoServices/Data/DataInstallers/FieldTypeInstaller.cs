using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using LinqIt.Cms.Data.DataInstallers;
using umbraco.cms.businesslogic.datatype;
using umbraco.cms.businesslogic.web;
using umbraco.interfaces;

namespace LinqIt.UmbracoServices.Data.DataInstallers
{
    public class FieldTypeInstaller : DataInstaller
    {
        private readonly UmbracoDataContext _context;
        private readonly Dictionary<string, DataTypeDefinition> _dataTypeDefinitions;
        
        public FieldTypeInstaller(UmbracoDataContext context)
        {
            _context = context;
            _dataTypeDefinitions = DataTypeDefinition.GetAll().ToDictionary(d => d.Text);
        }

        protected override void Install(System.Xml.XmlDocument data, StringBuilder log)
        {
            foreach (XmlElement fieldTypeElement in data.SelectNodes("snapshot/dataTypeDefinitions/datatypeDefinition"))
                InstallDataType(fieldTypeElement, log);
        }

        private void InstallDataType(XmlElement fieldTypeElement, StringBuilder log)
        {
            string name = fieldTypeElement.GetAttribute("name");
            log.AppendLine("");
            log.AppendLine("** Processing DataType Definition **" + name);
            if (!_dataTypeDefinitions.ContainsKey(name))
            {
                _dataTypeDefinitions.Add(name, CreateDefinition(name, log));
            }
            var df = _dataTypeDefinitions[name];
            if (df.Id < 0)
                return;

            var editorType = (IDataType)Activator.CreateInstance(Type.GetType(fieldTypeElement.GetAttribute("dataTypeClass")));
            editorType.DataTypeDefinitionId = df.Id;
            df.DataType = editorType;
            df.Save();

            UpdatePrevalues(fieldTypeElement, df, log);
            
            _context.SubmitChanges();
        }

        private void UpdatePrevalues(XmlElement fieldTypeElement, DataTypeDefinition df, StringBuilder log)
        {
            var prevalueHashA = new StringBuilder();
            foreach (var value in _context.cmsDataTypePreValues.Where(v => v.datatypeNodeId == df.Id).OrderBy(v => v.sortorder))
            {
                prevalueHashA.Append(value.alias);
                prevalueHashA.Append("|");
                prevalueHashA.Append(value.value);
                prevalueHashA.Append("|");
                prevalueHashA.Append(value.sortorder.ToString());
            }
            var prevalueHashB = new StringBuilder();
            foreach (XmlElement prevalue in fieldTypeElement.SelectNodes("preValue"))
            {
                prevalueHashB.Append(prevalue.HasAttribute("alias") ? prevalue.GetAttribute("alias") : string.Empty);
                prevalueHashB.Append("|");
                prevalueHashB.Append(prevalue.GetAttribute("value"));
                prevalueHashB.Append("|");
                prevalueHashB.Append(prevalue.GetAttribute("sortOrder"));
            }

            if (prevalueHashA.ToString() != prevalueHashB.ToString())
            {
                PreValues.DeleteByDataTypeDefinition(df.Id);
                foreach (XmlElement prevalue in fieldTypeElement.SelectNodes("preValue"))
                {
                    var value = new cmsDataTypePreValue();
                    value.datatypeNodeId = df.Id;
                    value.alias = prevalue.HasAttribute("alias") ? prevalue.GetAttribute("alias") : string.Empty;
                    value.datatypeNodeId = df.Id;
                    value.sortorder = Convert.ToInt32(prevalue.GetAttribute("sortOrder"));
                    value.value = prevalue.GetAttribute("value");
                    _context.cmsDataTypePreValues.InsertOnSubmit(value);
                    log.AppendLine("Updated prevalue: " + value.alias);
                }
            }
        }

        private static DataTypeDefinition CreateDefinition(string name, StringBuilder log)
        {
            var user = new umbraco.BusinessLogic.User("admin");
            var result = DataTypeDefinition.MakeNew(user, name);
            log.AppendLine("Created DataType Definition: " + name);
            return result;
        }
    }
}
