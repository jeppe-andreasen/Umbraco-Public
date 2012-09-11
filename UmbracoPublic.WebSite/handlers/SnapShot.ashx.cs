using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using Ionic.Zip;
using LinqIt.Cms;
using LinqIt.Cms.Data.DataIterators;
using umbraco.cms.businesslogic.web;

namespace UmbracoPublic.WebSite.handlers
{
    /// <summary>
    /// Summary description for SnapShot
    /// </summary>
    public class SnapShot : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/zip";

            var name = string.IsNullOrEmpty(context.Request.QueryString["name"]) ? "Package" : context.Request.QueryString["name"];
            context.Response.AddHeader("Content-Disposition", "attachment; filename=" + Path.GetFileNameWithoutExtension(name).Replace(" ", "") + ".zip");

            var preferences = PackagePreferences.Load();

            var options = new SnapShotOptions();
            options.From = preferences.From;
            options.To = preferences.To;
            options.InvalidPaths = preferences.InvalidPaths;
            options.ValidFileExtensions = preferences.ValidFileExtensions;
            
            var snapshot = GenerateSnapshotXml(options);

            var doc = new XmlDocument();
            doc.LoadXml(snapshot);
            var files = doc.SelectNodes("snapshot/files/file").Cast<XmlElement>().Select(e => e.InnerText).ToArray();
            var applicationPath = context.Server.MapPath("~/");

            using (var zipfile = new ZipFile(Encoding.UTF8))
            {
                zipfile.AddEntry("snapshot.xml", snapshot);

                foreach (var file in files)
                {
                    var archivePath = "Files\\" + Path.GetDirectoryName(file).Substring(applicationPath.Length).TrimEnd('\\');
                    zipfile.AddFile(file, archivePath);
                }

                zipfile.Save(context.Response.OutputStream);
            }
        }

        private static string GenerateSnapshotXml(SnapShotOptions options)
        {
            var builder = new StringBuilder();
            using (CmsContext.Editing)
            {
                using (var sw = new StringWriter(builder))
                using (var writer = new XmlTextWriter(sw))
                {
                    CmsService.Instance.BuildSnapShot(options, writer);
                }
            }
            return builder.ToString();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}