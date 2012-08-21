using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Xml;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.web;

namespace LinqIt.UmbracoServices.Data.DataInstallers
{
    public class SynchronizeTemplateTask : SynchronizationTask
    {
        private readonly XmlElement[] _input;
        private readonly string _connectionString;

        public SynchronizeTemplateTask(string connectionString, XmlElement[] templateElements)
        {
            _input = templateElements;
            _connectionString = connectionString;
        }

        protected override void AddMissingItems(StringBuilder log)
        {
            var existingTypes = DocumentType.GetAllAsList().ToDictionary(DataHelper.GetPath);
            foreach (var element in _input)
            {
                var path = element.GetAttribute("path");
                if (!existingTypes.ContainsKey(path))
                {
                    var name = element.GetAttribute("displayName");
                    var dt = DocumentType.MakeNew(new umbraco.BusinessLogic.User("admin"), name);
                    log.AppendLine("Created document type: " + name);
                    if (element.HasAttribute("parent"))
                    {
                        if (!existingTypes.ContainsKey(element.GetAttribute("parent")))
                            throw new ApplicationException("Parent not found : " + element.GetAttribute("parent"));
                        var parent = existingTypes[element.GetAttribute("parent")];
                        dt.MasterContentType = parent.Id;
                        log.AppendLine("Changed parent on document type. DT:" + path + ", Parent:" + element.GetAttribute("parent"));
                    }
                    dt.Save();
                    log.AppendLine("Saved document type: " + path);
                    ContentType.RemoveFromDataTypeCache(dt.Alias);
                    log.AppendLine("Removed document type from cache:" + path);
                    existingTypes.Add(path, dt);
                }
            }
        }

        protected override void UpdateItems(StringBuilder log)
        {
            var existingTypes = DocumentType.GetAllAsList().ToDictionary(DataHelper.GetPath);
            
            foreach (var element in _input)
            {
                var path = element.GetAttribute("path");
                if (!existingTypes.ContainsKey(path))
                    throw new ApplicationException("Document Type was not created: " + path);

                var dt = existingTypes[path];

                var alias = element.GetAttribute("alias");
                if (alias != dt.Alias)
                {
                    dt.Alias = alias;
                    log.AppendLine("Changed alias on document type. DT:" + path + ", Alias:" + alias);
                    dt.Save();
                    log.AppendLine("Document type saved: " + path);

                }
                UpdateDocTypeIconAndThumbnail(log, element, path, dt);
                ContentType.RemoveFromDataTypeCache(dt.Alias);
            }

            existingTypes = DocumentType.GetAllAsList().ToDictionary(DataHelper.GetPath);

            #region Set Allowed Child Types

            foreach (var element in _input)
            {
                var path = element.GetAttribute("path");
                var dt = existingTypes[path];
                var allowedChildDocumentTypes = element.SelectNodes("allowedChildren/add").Cast<XmlElement>().ToArray();
                var childtypesTask = new SynchronizeAllowedChildrenTask(allowedChildDocumentTypes, dt);
                childtypesTask.Process(log);
            }

            #endregion

            #region Create Tabs

            foreach (var element in _input)
            {
                var path = element.GetAttribute("path");
                var dt = existingTypes[path];

                var tabElements = element.SelectNodes("tabs/tab").Cast<XmlElement>().ToArray();
                var tabsTask = new SynchronizeTemplateTabsTask(_connectionString, tabElements, dt);
                tabsTask.Process(log);
            }

            #endregion

            

            //_datacontext.Refresh(RefreshMode.OverwriteCurrentValues, _datacontext.cmsTabs);

            #region Create Fields

            //foreach (var element in _input)
            //{
            //    var path = element.GetAttribute("path");
            //    var dt = existingTypes[path];

            //    var propertyElements = element.SelectNodes("properties/add").Cast<XmlElement>().ToArray();
            //    var propertiesTask = new SynchronizeTemplateFieldsTask(_connectionString, propertyElements, dt);
            //    propertiesTask.Process(log);
            //}

            #endregion
        }

        private void UpdateDocTypeIconAndThumbnail(StringBuilder log, XmlElement element, string path, DocumentType dt)
        {
            var changesMade = false;
            var dataContext = new UmbracoDataContext(_connectionString);

            var contentType = dataContext.cmsContentTypes.Where(t => t.nodeId == dt.Id).FirstOrDefault();
            if (contentType == null)
                throw new ApplicationException("ContentType does not exist: " + path);

            var icon = element.GetAttribute("icon");
            if (contentType.icon != icon)
            {
                contentType.icon = icon;
                changesMade = true;
                log.AppendLine("Changed icon on cmsContentType. CT:" + path + ", Icon:" + icon);
            }
            var thumbnail = element.GetAttribute("thumbnail");
            if (thumbnail != contentType.thumbnail)
            {
                contentType.thumbnail = thumbnail;
                changesMade = true;
                log.AppendLine("Changed thumbnail on cmsContentType. CT:" + path + ", Thumbnail:" + thumbnail);
            }

            if (changesMade)
            {
                dataContext.SubmitChanges();
            }
        }

        protected override void DeleteOldItems(StringBuilder log)
        {
            var existingTypes = DocumentType.GetAllAsList().ToDictionary(DataHelper.GetPath);
            foreach (var element in _input)
            {
                var path = element.GetAttribute("path");
                existingTypes.Remove(path);
            }

            var dataContext = new UmbracoDataContext(_connectionString);
            foreach (var dt in existingTypes.Values)
            {
                var contentType = dataContext.cmsContentTypes.Where(t => t.nodeId == dt.Id).FirstOrDefault();
                if (contentType.masterContentType == null)
                    continue;

                dt.delete();
            }
        }
    }
}
