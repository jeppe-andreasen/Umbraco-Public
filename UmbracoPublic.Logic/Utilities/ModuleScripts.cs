using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using LinqIt.Ajax.Parsing;
using LinqIt.Utils.Caching;
using LinqIt.Utils.Extensions;
using LinqIt.Utils.Web;

namespace UmbracoPublic.Logic.Utilities
{
    public static class ModuleScripts
    {
        public static void RegisterInitScript(string componentName, params JSONValue[] values)
        {
            if (values == null || !values.Any())
                ScriptUtil.RegisterInitScript(componentName, new string[0]);
            else
                ScriptUtil.RegisterInitScript(componentName, values.Select(v => v.ToString()).ToArray());
        }

        public static string GetInitializationScript()
        {
            return ScriptUtil.Instance.GetInitializationScript();
        }
    }
}

