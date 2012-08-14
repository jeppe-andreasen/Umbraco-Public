using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using umbraco.cms.businesslogic.datatype;
using umbraco.cms.businesslogic.web;

namespace UmbracoPublic.Logic.Utilities
{
    public static class ModuleHelper
    {
        public static void CreateModule(string name, Dictionary<string, int> properties)
        {
            GenerateTemplate(name, properties);

            GenerateEntity(name, properties);

            GenerateUserControl(name, properties);
        }

        private static void GenerateUserControl(string name, Dictionary<string, int> properties)
        {
            string moduleFolder = HttpContext.Current.Server.MapPath("~/modules");

            CreateFile(moduleFolder, name, "Rendering.ascx", Properties.Resources.ModuleAscxTemplate);
            CreateFile(moduleFolder, name, "Rendering.ascx.cs", Properties.Resources.ModuleCodeTemplate);
            CreateFile(moduleFolder, name, "Rendering.ascx.designer.cs", Properties.Resources.ModuleDesignerTemplate);
        }

        private static void CreateFile(string moduleFolder, string name, string extension, string content)
        {
            string filename = moduleFolder + @"\" + name + extension;
            using (var writer = new StreamWriter(filename, false))
            {
                writer.Write(content.Replace("MODULE_NAME", name));
            }
        }

        private static void GenerateEntity(string name, Dictionary<string, int> properties)
        {
            var rootFolder = HttpContext.Current.Server.MapPath("~/");
            var rootInfo = new DirectoryInfo(rootFolder);
            var filepath = rootInfo.Parent.FullName + @"\UmbracoPublic.Logic\Entities\" + name.Replace(" ", "") + ".cs";
            using (var writer = new StreamWriter(filepath, false))
            {
                writer.WriteLine("using System;");
                writer.WriteLine("using System.Collections.Generic;");
                writer.WriteLine("using System.Linq;");
                writer.WriteLine("using System.Text;");
                writer.WriteLine("using LinqIt.Cms.Data;");
                writer.WriteLine("using LinqIt.Components.Data;");
                writer.WriteLine("namespace UmbracoPublic.Logic.Entities");
                writer.WriteLine("{");
                writer.WriteLine("\tpublic class " + name.Replace(" ", "") + " : Entity");
                writer.WriteLine("\t{");
                writer.WriteLine("\t}");
                writer.WriteLine("}");
            }
        }

        private static void GenerateTemplate(string name, Dictionary<string, int> properties)
        {
            var user = new umbraco.BusinessLogic.User("admin");
            var parentTemplate = DocumentType.GetByAlias("GridModule");
            DocumentType template = DocumentType.MakeNew(user, name);

            int tabId = template.AddVirtualTab("Content");
            foreach (string key in properties.Keys)
            {
                var property = template.AddPropertyType(new DataTypeDefinition(properties[key]), (Char.ToLower(key[0]) + key.Substring(1)).Replace(" ", ""), key);
                template.SetTabOnPropertyType(property, tabId);
            }
            template.Save();

            #region Set Master Document type

            SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["umbracoDbDSN"]);
            connection.Open();

            SqlCommand command = new SqlCommand(string.Format("UPDATE cmsContentType SET masterContentType = {0} WHERE nodeId= {1}", parentTemplate.Id, template.Id), connection);
            command.ExecuteNonQuery();

            connection.Close();

            #endregion
        }
    }
}
