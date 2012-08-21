using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using umbraco.cms.businesslogic.web;

namespace LinqIt.UmbracoServices.Data.DataInstallers
{
    public class SynchronizeAllowedChildrenTask : SynchronizationTask
    {
        private readonly XmlElement[] _input;
        private readonly DocumentType _container;
        private readonly Dictionary<string, int> _documentTypeIds;

        public SynchronizeAllowedChildrenTask(XmlElement[] input, DocumentType container)
        {
            _input = input;
            _container = container;
            _documentTypeIds = DocumentType.GetAllAsList().ToDictionary(DataHelper.GetPath, dt => dt.Id);
        }

        protected override void AddMissingItems(StringBuilder log)
        {
            
        }

        protected override void UpdateItems(StringBuilder log)
        {
            _container.AllowedChildContentTypeIDs = _input.Select(i => _documentTypeIds[i.InnerText]).ToArray();
            log.AppendLine("Set alloed child content on DT: " + _container.Text);
            _container.Save();
            log.AppendLine("Saved document type: " + _container.Text);
        }

        protected override void DeleteOldItems(StringBuilder log)
        {
            
        }
    }
}
