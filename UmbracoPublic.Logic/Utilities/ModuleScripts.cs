using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using LinqIt.Utils.Caching;
using LinqIt.Utils.Extensions;

namespace UmbracoPublic.Logic.Utilities
{
    public class ModuleScripts
    {
        private readonly Dictionary<String, String> _initializationLines;

        #region Constructors

        private ModuleScripts()
        {
            _initializationLines = new Dictionary<string, string>(); 
        }

        #endregion Constructors

        public static ModuleScripts Instance
        {
            get
            {
                return Cache.Get(typeof(ModuleScripts).FullName, CacheScope.Request, () => new ModuleScripts());
            }
        }

        /// <summary>
        /// Return the full init script with all script tags
        /// </summary>
        /// <returns></returns>
        public String GetInitializationScript()
        {
            if (!_initializationLines.Any())
                return string.Empty;

            var builder = new StringBuilder();
            builder.AppendLine("<script type=\"text/javascript\">");
            foreach (var script in _initializationLines.Values)
            {
                builder.Append(script);
                if (!script.EndsWith(";"))
                    builder.AppendLine(";");
                else
                    builder.AppendLine();
            }
            builder.AppendLine();
            builder.AppendLine("</script>");
            return builder.ToString();
        }

        public void RegisterInitializationScripts(UserControl control, params string[] scripts)
        {
            var key = control.GetType().BaseType.Name;
            if (_initializationLines.ContainsKey(key))
                return;
            _initializationLines.Add(key, scripts.ToSeparatedString("\r\n"));
        }

        public void RegisterInitializationScript(Page page, params string[] scripts)
        {
            var key = page.GetType().Name;
            if (_initializationLines.ContainsKey(key))
                return;
            _initializationLines.Add(key, scripts.ToSeparatedString("\r\n"));
        }
    }
}

