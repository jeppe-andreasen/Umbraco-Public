using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace UmbracoPublic.WebSite.handlers
{
    public class PackagePreferences
    {
        public const string DateTimeFormat = "dd.MM.yyyy HH:mm";
        private string[] _invalidPaths = new string[0];
        private string[] _validExtensions = new string[0];

        public static PackagePreferences Load()
        {
            var filename = GetPreferencesPath();
            if (!File.Exists(filename))
                return null;

            var doc = new XmlDocument();
            doc.Load(filename);

            var result = new PackagePreferences();
            var config = doc.DocumentElement;
            if (config != null)
            {
                result.From = ParseDateTime(config["from"].InnerText);
                result.To = ParseDateTime(config["to"].InnerText);
                result.LastBuildTime = ParseDateTime(config["lastBuildTime"].InnerText);

                result.ValidFileExtensions = ParseStringList(config["validFileExtensions"]);
                result.InvalidPaths = ParseStringList(config["invalidPaths"]);
            }

            return result;
        }

        private static string[] ParseStringList(XmlNode xmlElement)
        {
            return xmlElement.ChildNodes.Cast<XmlElement>().Select(e => e.InnerText).ToArray();
        }

        public void Save()
        {
            var filename = GetPreferencesPath();
            if (!Directory.Exists(Path.GetDirectoryName(filename)))
                Directory.CreateDirectory(Path.GetDirectoryName(filename));

            if (File.Exists(filename))
                File.Delete(filename);

            using (var writer = new XmlTextWriter(filename, Encoding.UTF8))
            {
                writer.WriteStartElement("config");
                writer.WriteElementString("from", From.ToString(DateTimeFormat));
                writer.WriteElementString("to", To.ToString(DateTimeFormat));
                writer.WriteElementString("lastBuildTime", (LastBuildTime ?? DateTime.Now).ToString(DateTimeFormat));
                
                writer.WriteStartElement("validFileExtensions");
                if (ValidFileExtensions != null)
                {
                    foreach (var ext in ValidFileExtensions)
                        writer.WriteElementString("add", ext);
                }
                writer.WriteEndElement();

                
                writer.WriteStartElement("invalidPaths");
                if (InvalidPaths != null)
                {
                    foreach (var path in InvalidPaths)
                        writer.WriteElementString("add", path);
                }
                writer.WriteEndElement(); // invalidPaths

                writer.WriteEndElement(); // root
            }
        }

        private static string GetPreferencesPath()
        {
            string filename = System.Environment.MachineName + ".xml";
            return Path.Combine(HttpContext.Current.Server.MapPath("~/App_Data/Preferences"), filename);
        }

        public static DateTime ParseDateTime(string value)
        {
            return DateTime.ParseExact(value, DateTimeFormat, CultureInfo.InvariantCulture);
        }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public DateTime? LastBuildTime { get; set; }

        public string[] ValidFileExtensions
        {
            get { return _validExtensions; }
            set
            {
                _validExtensions = value == null ? new string[0] : value.Where(s => !string.IsNullOrEmpty(s)).Select(s => s.ToLower()).ToArray();
            }
        }

        public string[] InvalidPaths
        {
            get
            {
                return _invalidPaths;
            }
            set 
            {
                _invalidPaths = value == null ? new string[0] : value.Where(s => !string.IsNullOrEmpty(s)).Select(s => s.ToLower()).ToArray();
            }
        }

        internal bool IsInvalidPath(string path)
        {
            return string.IsNullOrEmpty(path) || _invalidPaths.Contains(path.ToLower());
        }

        internal bool IsValidExtension(string ext)
        {
            return !string.IsNullOrEmpty(ext) && _validExtensions.Contains(ext);
        }
    }
}