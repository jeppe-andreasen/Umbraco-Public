using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LinqIt.Ajax.Parsing;
using LinqIt.Utils.Web;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.WebSite.usercontrols.Parts
{
    public class BasePart : System.Web.UI.UserControl
    {
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            string content = HtmlWriter.Generate(RenderPart);
            if (!string.IsNullOrEmpty(content))
                writer.Write(content);
            base.Render(writer);
        }

        protected virtual void RenderPart(HtmlWriter writer)
        {
        }

        protected void RegisterScriptInit(string partName, params JSONValue[] values)
        {
            string script = "application." + partName + ".init(";
            var parameters = string.Empty;
            foreach (var value in values)
            {
                if (!string.IsNullOrEmpty(parameters))
                    parameters += ",";
                parameters += value.ToString();
            }
            script += parameters + ");";
            ModuleScripts.Instance.RegisterInitializationScripts(this, script);
        }
    }
}