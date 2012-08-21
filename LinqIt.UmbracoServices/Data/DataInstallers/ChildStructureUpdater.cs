using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using LinqIt.Utils.Extensions;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.web;

namespace LinqIt.UmbracoServices.Data.DataInstallers
{
    public class ChildStructureUpdater
    {
        private readonly DocumentType _documentType;
        private readonly XmlElement _element;

        public ChildStructureUpdater(DocumentType documentType, XmlElement element)
        {
            _documentType = documentType;
            _element = element;
        }

        internal void Process(StringBuilder log, Dictionary<string, DocumentType> documentTypeLookup)
        {
            bool requiresSave = false;
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
                requiresSave = true;
                log.AppendLine("SET DT Structure: " + _documentType.Text + " -> " + oldHash + " => " + newHash);
            }

            if (requiresSave)
            {
                _documentType.Save();
                ContentType.RemoveFromDataTypeCache(_documentType.Alias);
            }
        }

        private static string GetHash(IEnumerable<int> items)
        {
            return items.OrderBy(i => i).ToSeparatedString(",");
        }
    }
}
