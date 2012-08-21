using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using LinqIt.Cms;

namespace UmbracoPublic.Logic.Entities
{
    public class Theme
    {
        public Theme(XmlDocument configuration)
        {
            Stylesheets = SelectNodes(configuration, "stylesheets");
            Javascripts = SelectNodes(configuration, "javascripts");
        }

        public Theme(string themeFolder)
        {
            string applicationFolder = HttpContext.Current.Server.MapPath("~/");
            var cssFolder = Path.Combine(themeFolder, "css");
            if (Directory.Exists(cssFolder))
            {
                Stylesheets = Directory.GetFiles(cssFolder, "*.css").Select(p => MakeRelative(p, applicationFolder)).ToList();
            }
            else
            {
                Stylesheets = Directory.GetFiles(themeFolder, "*.css").Select(p => MakeRelative(p, applicationFolder)).ToList();
            }
        }

        private static string MakeRelative(string path, string applicationPath)
        {
            return path.Substring(applicationPath.Length).Replace(@"\", "/");
        }
        

        private static List<string> SelectNodes(XmlDocument configuration, string section)
        {
            var nodes = configuration.SelectNodes("config/" + section + "/add");
            return nodes == null ? new List<string>() : nodes.Cast<XmlElement>().Select(e => e.GetAttribute("path")).ToList();
        }

        public List<string> Stylesheets { get; private set; }

        public List<string> Javascripts { get; private set; }

        public static Theme Current
        {
            get
            {
                var virtualPath = HttpContext.Current.Server.MapPath("~/Themes");
                var siteRoot = CmsService.Instance.GetItem<SiteRoot>(CmsService.Instance.SitePath);

                if (siteRoot == null)
                    return null;

                var themeFolder = Path.Combine(virtualPath, siteRoot.Theme);
                if (!Directory.Exists(themeFolder))
                    return null;

                var configPath = Path.Combine(themeFolder, "config.xml");
                if (!File.Exists(configPath))
                    return new Theme(themeFolder);
                try
                {
                    var doc = new XmlDocument();
                    doc.Load(configPath);
                    return new Theme(doc);
                }
                catch
                {
                    throw new ApplicationException("Unable to load theme configuration file");
                }
            }
        }
    }
}
