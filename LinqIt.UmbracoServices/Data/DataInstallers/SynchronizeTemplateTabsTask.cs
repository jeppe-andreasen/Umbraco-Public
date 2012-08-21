using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.web;

namespace LinqIt.UmbracoServices.Data.DataInstallers
{
    public class SynchronizeTemplateTabsTask : SynchronizationTask
    {
        private readonly XmlElement[] _input;
        private DocumentType _container;
        private readonly string _connectionString;

        public SynchronizeTemplateTabsTask(string connectionString, XmlElement[] tabElements, DocumentType container)
        {
            _input = tabElements;
            _container = container;
            _connectionString = connectionString;
        }

        protected override void AddMissingItems(StringBuilder log)
        {
            var existingTabNames = _container.getVirtualTabs.Select(t => t.Caption).ToList();
            bool updateMade = false;
            foreach (var element in _input)
            {
                var name = element.GetAttribute("name");
                if (!existingTabNames.Contains(name))
                {
                    _container.AddVirtualTab(name);
                    log.AppendLine("Added virtual tab. DT: " + _container.Text + ", Tab:" + name);
                    updateMade = true;
                }
            }
            if (updateMade)
                PersistContainer(log);
        }

        protected override void UpdateItems(StringBuilder log)
        {
            var changesMade = false;
            UmbracoDataContext dataContext = null;
            foreach (var element in _input)
            {
                var name = element.GetAttribute("name");
                var sortOrder = Convert.ToInt32(element.GetAttribute("sortOrder"));
                var tab = _container.getVirtualTabs.Where(t => t.Caption == name).FirstOrDefault();
                if (tab == null)
                    throw new ApplicationException("Tab was not created or cache not updated: " + name);

                if (tab.SortOrder != sortOrder)
                {
                    if (dataContext == null)
                        dataContext = new UmbracoDataContext(_connectionString);
                    var cmsTab = dataContext.cmsTabs.Where(t => t.contenttypeNodeId == _container.Id && t.text == name).FirstOrDefault();
                    if (cmsTab == null)
                        throw new ApplicationException("Tab not yet in database: " + name);
                    cmsTab.sortorder = sortOrder;
                    changesMade = true;
                    log.AppendLine("Changed sortorder on tab. DT:" + _container.Text + ", Tab: " + name);
                }
            }

            if (changesMade)
            {
                dataContext.SubmitChanges();
                log.AppendLine("Submitted Changes");
                RefreshContainer(log);
            }
        }

        private void RefreshContainer(StringBuilder log)
        {
            ContentType.RemoveFromDataTypeCache(_container.Alias);
            log.AppendLine("Removed documenttype from cache: " + _container.Text);
            _container = new DocumentType(_container.Id);
        }

        private void PersistContainer(StringBuilder log)
        {
            _container.Save();
            log.AppendLine("Saved document type: " + _container.Text);
            RefreshContainer(log);
        }

        protected override void DeleteOldItems(StringBuilder log)
        {
            var existingTabs = _container.getVirtualTabs.ToDictionary(t => t.Caption);
            foreach (var element in _input)
            {
                var name = element.GetAttribute("name");
                existingTabs.Remove(name);
            }
            bool changesMade = false;
            foreach (var tab in existingTabs.Values)
            {
                log.AppendLine("Deleted tab. DT:" + _container.Text + ", " + tab.Caption);
                _container.DeleteVirtualTab(tab.Id);
                changesMade = true;
            }
            if (changesMade)
                PersistContainer(log);
        }
    }
}
