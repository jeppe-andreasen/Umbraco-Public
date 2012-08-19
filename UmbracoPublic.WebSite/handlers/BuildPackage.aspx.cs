using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
using LinqIt.Cms;
using LinqIt.Cms.Data.DataIterators;
using LinqIt.Utils;

namespace UmbracoPublic.WebSite.handlers
{
    public partial class BuildPackage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var preferences = PackagePreferences.Load();
                PopulateExtensions(preferences);
                if (preferences != null)
                    txtDate.Text = preferences.LastBuildTime.Value.ToString(PackagePreferences.DateTimeFormat);
                else
                    txtDate.Text = DateTime.Today.ToString(PackagePreferences.DateTimeFormat);
            }
        }

        private void PopulateExtensions(PackagePreferences preferences)
        {
            var root = Server.MapPath("~/");
            
            cblExtensions.Items.AddRange(
                IterationUtil.FindAllBFS(root, GetSubFiles, IsFile).
                    Select(f => (Path.GetExtension(f) ?? "").ToLower()).
                    Distinct().
                    Where(e => !string.IsNullOrEmpty(e)).
                    OrderBy(e => e).
                    Select(e => new ListItem(e, e){Selected = preferences != null && preferences.IsValidExtension(e)}).
                    ToArray());

            
        }

        private static bool IsFile(string path)
        {
            return (File.GetAttributes(path) & FileAttributes.Directory) != FileAttributes.Directory;
        }

        private static IEnumerable<string> GetSubFiles(string path)
        {
            return IsFile(path) ? new string[0] : Directory.GetDirectories(path).Union(Directory.GetFiles(path, "*.*"));
        }

        protected void OnGetSnapshotClicked(object sender, EventArgs e)
        {
            var builder = new StringBuilder();
            using (CmsContext.Editing)
            {
                using (var sw = new StringWriter(builder))
                using (var writer = new XmlTextWriter(sw))
                {
                    var options = new SnapShotOptions();
                    options.From = PackagePreferences.ParseDateTime(txtDate.Text);
                    options.To = DateTime.Now;
                    options.ValidFileExtensions = cblExtensions.Items.Cast<ListItem>().Where(i => i.Selected).Select(i => i.Value).ToArray();
                    CmsService.Instance.BuildSnapShot(options, writer);
                }
            }
            
            var snapshot = new XmlDocument();
            snapshot.LoadXml(builder.ToString());

            TreeView1.Nodes.Clear();

            var preferences = PackagePreferences.Load();

            AddFiles(snapshot, preferences);
            AddDataTypes(snapshot, preferences);
            AddTemplates(snapshot, preferences);
        }

        private void AddDataTypes(XmlDocument snapshot, PackagePreferences preferences)
        {
            var root = GetTreeNode("Field Types", "fieldtypes", TreeView1.Nodes, preferences);
            foreach (XmlElement element in snapshot.SelectNodes("snapshot/dataTypeDefinitions/datatypeDefinition"))
            {
                var name = element.GetAttribute("name");
                var node = GetTreeNode(name, name.ToLower(), root.ChildNodes, preferences);
                node.ImageUrl = "/umbraco/images/umbraco/developerDatatype.gif";
            }
        }

        private void AddTemplates(XmlDocument snapshot, PackagePreferences preferences)
        {
            var root = GetTreeNode("Templates", "templates", TreeView1.Nodes, preferences);
            foreach (XmlElement element in snapshot.SelectNodes("snapshot/templates/template"))
            {
                var name = element.GetAttribute("displayName");
                var alias = element.GetAttribute("alias");
                var node = GetTreeNode(name, alias.ToLower(), root.ChildNodes, preferences);
                node.ImageUrl = "/umbraco/images/umbraco/settingDataType.gif";
            }
        }

        private void AddFiles(XmlDocument snapshot, PackagePreferences preferences)
        {
            var filesRoot = GetTreeNode("Files", "files", TreeView1.Nodes, preferences);
            var rootDir = Server.MapPath("~/").TrimEnd('\\');

            foreach (var filename in snapshot.SelectNodes("snapshot/files/file").Cast<XmlElement>().Select(e => e.InnerText).OrderBy(f => f))
            {
                var parts = filename.Substring(rootDir.Length).Trim('\\').Split('\\');
                var root = filesRoot;
                var partFilename = rootDir;
                foreach(var part in parts)
                {
                    partFilename += "\\" + part;

                    var node = root.ChildNodes.Cast<TreeNode>().Where(n => n.Value == part).FirstOrDefault();
                    if (node == null)
                    {
                        var fileQuery = IsFile(partFilename) ? Path.GetExtension(partFilename) : partFilename;
                        node = GetTreeNode(part, part.ToLower(), root.ChildNodes, preferences);
                        node.ImageUrl = "/geticon.axd?file=" + HttpUtility.UrlEncode(fileQuery) + "&size=small";
                    }
                    root = node;
                }
            }
        }

        private static TreeNode GetTreeNode(string name, string value, TreeNodeCollection parent, PackagePreferences preferences)
        {
            TreeNode result = new TreeNode(name, value);
            result.ShowCheckBox = true;
            parent.Add(result);
            result.Checked = preferences == null || !preferences.IsInvalidPath(result.ValuePath);
            return result;
        }

        protected void OnGeneratePackageClicked(object sender, EventArgs e)
        {
            var preferences = new PackagePreferences();
            preferences.From = PackagePreferences.ParseDateTime(txtDate.Text);
            preferences.To = DateTime.Now;
            preferences.ValidFileExtensions = cblExtensions.Items.Cast<ListItem>().Where(li => li.Selected).Select(li => li.Value).ToArray();
            var invalidPaths = new List<string>();
            foreach (TreeNode node in TreeView1.Nodes)
                UpdateInvalidPaths(invalidPaths, node);
            preferences.InvalidPaths = invalidPaths.ToArray();
            preferences.Save();
            Response.Redirect("/handlers/SnapShot.ashx");
        }

        private static void UpdateInvalidPaths(ICollection<string> invalidPaths, TreeNode node)
        {
            if (!node.Checked)
                invalidPaths.Add(node.ValuePath);
            foreach (TreeNode child in node.ChildNodes)
                UpdateInvalidPaths(invalidPaths, child);
        }
    }
}