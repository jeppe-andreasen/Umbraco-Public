using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using LinqIt.Ajax.Parsing;
using LinqIt.Utils.Caching;
using LinqIt.Utils.Extensions;

namespace UmbracoPublic.Logic.Utilities
{
    public class ModuleScripts
    {
        private readonly List<string> _initializationLines = new List<string>();

        #region Constructors

        private ModuleScripts()
        {
            
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
            foreach (var script in _initializationLines)
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

        public static void RegisterInitScript(string componentName, params JSONValue[] values)
        {
            Instance.AddScript(componentName, values);
        }

        private void AddScript(string componentName, params JSONValue[] values)
        {
            var parameters = values != null ? values.ToSeparatedString(",") : string.Empty;
            var script = "application." + componentName + ".init(" + parameters + ");";
            if (!_initializationLines.Contains(script))
                _initializationLines.Add(script);
        }
    }
}

