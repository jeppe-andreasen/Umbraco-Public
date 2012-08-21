using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.web;

namespace LinqIt.UmbracoServices.Data.DataInstallers
{
    public class TemplateCreator
    {
        private string _connectionString;
        private readonly XmlElement _element;

        public TemplateCreator(string connectionString, XmlElement element)
        {
            _connectionString = connectionString;
            _element = element;
        }

        internal void Process(StringBuilder log, Dictionary<string, DocumentType> documentTypeLookup)
        {
            string name = _element.GetAttribute("displayName");

            log.AppendLine("");
            log.AppendLine("** Creating document type " + name + " **");

            var user = new User("admin");
            var documentType = DocumentType.MakeNew(user, _element.GetAttribute("displayName"));
            log.AppendLine("NEW DT: " + documentType.Text);

            var parentPath = _element.HasAttribute("parent") ? _element.GetAttribute("parent") : null;
            if (!string.IsNullOrEmpty(parentPath))
            {
                if (!documentTypeLookup.ContainsKey(parentPath))
                    throw new ApplicationException("Parent not found : " + parentPath);
                var parent = documentTypeLookup[parentPath];
                documentType.MasterContentType = parent.Id;
                log.AppendLine("SET DT Parent: " + documentType.Text + " -> " + parent.Text);
            }

            documentType.Alias = _element.GetAttribute("alias");
            documentType.Save();
        }
    }
}
