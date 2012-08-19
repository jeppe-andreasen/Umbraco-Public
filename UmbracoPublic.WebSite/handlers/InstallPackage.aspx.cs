using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Ionic.Zip;
using LinqIt.Cms;

namespace UmbracoPublic.WebSite.handlers
{
    public partial class InstallPackage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void OnInstallClicked(object sender, EventArgs e)
        {
            if (ulPackage.HasFile)
            {
                var folder = PrepareDeploymentFolder();
                var filename = Path.Combine(folder, "package.zip");
                ulPackage.SaveAs(filename);

                using (var zipFile = new ZipFile(filename, Encoding.UTF8))
                {
                    zipFile.ExtractAll(folder);   
                }
                InstallPackageContents(folder);
            }
        }

        private void InstallPackageContents(string folder)
        {
            var document = new XmlDocument();
            document.Load(Path.Combine(folder, "SnapShot.xml"));

            var log = new StringBuilder();
            try
            {
                var rootPath = Server.MapPath("~/");
                var initialDir = Path.Combine(folder, "Files");
                if (Directory.Exists(initialDir))
                    CopyDirectory(rootPath, initialDir, log);

                CmsService.Instance.InstallSnapShot(document, log);
            }
            catch (Exception exc)
            {
                log.AppendLine("** Error **");
                log.AppendLine(exc.ToString());
            }
            litOutput.Text = log.ToString().Replace("\r\n", "<br>");
            multiview.ActiveViewIndex = 1;

            CleanupDeploymentFolder(folder);
        }

        private void CopyDirectory(string rootPath, string currentDirectory, StringBuilder log)
        {
            foreach (var dir in Directory.GetDirectories(currentDirectory))
                CopyDirectory(rootPath, dir, log);

            var relativeDir = currentDirectory.Substring(Path.Combine(rootPath, "App_Data/Deployment/Files").Length);
            var absoluteDir = CombinePaths(rootPath, relativeDir);
            if (!Directory.Exists(absoluteDir))
                Directory.CreateDirectory(absoluteDir);

            foreach (var file in Directory.GetFiles(currentDirectory, "*.*"))
            {
                var destinationFilename = CombinePaths(absoluteDir, Path.GetFileName(file));
                var relativeFilename = "~/" + CombinePaths(relativeDir, Path.GetFileName(file)).Replace('\\', '/');
                try
                {
                    //File.Copy(file, Path.Combine(absoluteDir, destinationFilename), true);
                    log.AppendLine("Copied " + relativeFilename);
                    //log.AppendLine("Copied " + destinationFilename);
                }
                catch (Exception exc)
                {
                    log.AppendLine("Error copying " + relativeFilename + ", " + exc.Message);
                }
            }
        }

        private string CombinePaths(params string[] paths)
        {
            var result = new StringBuilder();
            foreach (var path in paths)
            {
                if (result.Length > 0)
                    result.Append("\\");
                result.Append(path.Trim('\\'));
            }
            return result.ToString();
        }

        private string PrepareDeploymentFolder()
        {
            var deploymentFolder = Server.MapPath("~/App_Data/Deployment");
            if (!Directory.Exists(deploymentFolder))
                Directory.CreateDirectory(deploymentFolder);

            CleanupDeploymentFolder(deploymentFolder);
            return deploymentFolder;
        }

        private static void CleanupDeploymentFolder(string deploymentFolder)
        {
            foreach (var file in Directory.GetFiles(deploymentFolder, "*.*"))
                File.Delete(file);
            foreach (var directory in Directory.GetDirectories(deploymentFolder))
                Directory.Delete(directory, true);
        }
    }
}