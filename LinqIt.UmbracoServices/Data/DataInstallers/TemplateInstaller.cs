using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using LinqIt.Cms.Data.DataInstallers;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.web;

namespace LinqIt.UmbracoServices.Data.DataInstallers
{
    public class TemplateInstaller : DataInstaller
    {
        private readonly string _connectionString;

        public TemplateInstaller(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void Install(System.Xml.XmlDocument data, StringBuilder log)
        {
            var templateElements = data.SelectNodes("snapshot/templates/template").Cast<XmlElement>().ToArray();
            var existingDocumentTypes = DocumentType.GetAllAsList().ToDictionary(d => d.Alias);
            var documentTypeLookup = new Dictionary<string, DocumentType>();
            foreach (var element in templateElements)
            {
                var alias = element.GetAttribute("alias");
                if (existingDocumentTypes.ContainsKey(alias))
                    new TemplateUpdater(existingDocumentTypes[alias], _connectionString, element).UpdateTemplate(log, documentTypeLookup);
                else
                {
                    new TemplateCreator(_connectionString, element).Process(log, documentTypeLookup);
                    ContentType.RemoveFromDataTypeCache(alias);
                    new TemplateUpdater(DocumentType.GetByAlias(alias), _connectionString, element).UpdateTemplate(log, documentTypeLookup);
                }
                ContentType.RemoveFromDataTypeCache(alias);
                var documentType = DocumentType.GetByAlias(alias);
                documentTypeLookup.Add(DataHelper.GetPath(documentType), documentType);
                existingDocumentTypes.Remove(alias);
            }

            foreach (var element in templateElements)
            {
                var alias = element.GetAttribute("alias");
                ContentType.RemoveFromDataTypeCache(alias);
                var updater = new TemplateUpdater(DocumentType.GetByAlias(alias), _connectionString, element);
                updater.UpdateChildStrucure(log, documentTypeLookup);
                updater.UpdateTabs(log, documentTypeLookup);
                
            }
            foreach (var element in templateElements)
            {
                var alias = element.GetAttribute("alias");
                ContentType.RemoveFromDataTypeCache(alias);
                var updater = new TemplateUpdater(DocumentType.GetByAlias(alias), _connectionString, element);
                updater.UpdateProperties(log, documentTypeLookup);
            }
        }

        //private void InstallTemplate(XmlElement templateElement)
        //{
        //    var path = templateElement.GetAttribute("path");

        //    DocumentType dt = DocumentType.GetAllAsList().Where(t => t.Alias == templateElement.GetAttribute("alias")).FirstOrDefault();
        //    if (dt == null)
        //    {
        //        var user = new umbraco.BusinessLogic.User("admin");
        //        dt = DocumentType.MakeNew(user, templateElement.GetAttribute("name"));
        //        dt.Alias = templateElement.GetAttribute("alias");
        //        dt.Save();

        //        var existingTabs = dt.getVirtualTabs.ToDictionary(t => t.Caption);
        //        foreach (XmlElement tabElement in templateElement.SelectNodes("tabs/tab"))
        //        {
        //            if (!existingTabs.ContainsKey(tabElement.GetAttribute("name")))
        //                dt.AddVirtualTab(tabElement.GetAttribute("name"));
        //        }
        //        dt.Save();
        //    }

        //    cmsContentType template = _contentTypes.ContainsKey(path) ? _contentTypes[path] : CreateContentType(path, templateElement);
            
        //    template.icon = templateElement.GetAttribute("icon");
        //    template.thumbnail = templateElement.GetAttribute("thumbnail");
        //    if (templateElement.HasAttribute("parent"))
        //        template.masterContentType = _contentTypes[templateElement.GetAttribute("parent")].nodeId;
        //    else
        //        template.masterContentType = 0;

        //    CreateMissingTabs(templateElement, template);

        //    foreach (XmlElement property in templateElement.SelectNodes("properties/add"))
        //    {
        //        string alias = property.GetAttribute("alias");
        //        var field = template.cmsPropertyTypes.Where(pt => pt.Alias == alias).FirstOrDefault();
        //        if (field == null)
        //        {
                    
                    
        //        }
        //    }
        //    _context.SubmitChanges();



        //}

        //private void CreateMissingTabs(XmlElement templateElement, cmsContentType template)
        //{
        //    foreach (XmlElement tab in templateElement.SelectNodes("tabs/tab"))
        //    {
        //        string tabPath = GetPath(template) + "/" + tab.GetAttribute("name");
        //        if (!_tabs.ContainsKey(tabPath))
        //        {
        //            cmsTab t = new cmsTab();
        //            t.contenttypeNodeId = template.nodeId;
        //            t.text = tab.GetAttribute("name");
        //            t.sortorder = Convert.ToInt32(tab.GetAttribute("sortOrder"));
        //            _context.cmsTabs.InsertOnSubmit(t);
        //            _tabs.Add(tabPath, t);
        //        }
        //    }
        //}

        //private cmsContentType CreateContentType(string path, XmlElement templateElement)
        //{
        //    var user = new umbraco.BusinessLogic.User("admin");
        //    var documentType = DocumentType.MakeNew(user, templateElement.GetAttribute("name"));
        //    documentType.Alias = templateElement.GetAttribute("alias");
        //    documentType.Save();

        //    var result = _context.cmsContentTypes.Where(c => c.nodeId == documentType.Id).FirstOrDefault();
        //    _contentTypes.Add(GetPath(result), result);
        //    return result;
        //}

        //private string GetPath(cmsContentType template)
        //{
        //    if (template.masterContentType != 0)
        //    {
        //        var parent = _context.cmsContentTypes.Where(t => t.nodeId == template.masterContentType).FirstOrDefault();
        //        if (parent != null)
        //            return GetPath(parent) + "/" + template.alias;
        //    }
        //    return template.alias;
        //}
    }

    
}
