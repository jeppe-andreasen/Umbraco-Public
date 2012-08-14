using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using LinqIt.Components;
using LinqIt.Utils.Extensions;
using LinqIt.Utils.Web;
using umbraco.cms.businesslogic.macro;

namespace UmbracoPublic.Logic.Utilities
{
    public class HtmlContent
    {
        public static void CreateControls(Action<HtmlWriter> rendering, PlaceHolder placeholder)
        {
            string html = HtmlWriter.Generate(rendering);
            if (string.IsNullOrEmpty(html))
                return;

            var regex = new Regex(@"<\?UMBRACO_MACRO(?<property>\s*(?<name>[a-zA-Z_]+)=""(?<value>[^""]*)"")+\s*/>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            foreach (var extraction in regex.Extract(html))
            {
                if (extraction.Type == RegexExtractionType.Text)
                    placeholder.Controls.Add(new LiteralControl(((RegexTextExtraction)extraction).Value));
                else
                    placeholder.Controls.Add(LoadMacro(((RegexMatchExtraction)extraction).Value));
            }
        }

        public static void CreateModuleContent(Action<HtmlWriter> rendering, PlaceHolder placeholder, int? columnSpan)
        {
            string html = HtmlWriter.Generate(rendering);
            if (string.IsNullOrEmpty(html))
                return;

            var regex = new Regex(@"<module ref=""(?<ref>\d+)""[^/]*/[^>]*>", RegexOptions.Singleline);

            foreach (var extraction in regex.Extract(html))
            {
                if (extraction.Type == RegexExtractionType.Text)
                    placeholder.Controls.Add(new LiteralControl(((RegexTextExtraction)extraction).Value));
                else
                    placeholder.Controls.Add(LoadModule(((RegexMatchExtraction)extraction).Value, columnSpan));
            }
        }

        private static Control LoadMacro(Match match)
        {
            if (match == null)
                return null;

            var parameters = new Dictionary<string, string>();
            for (var i = 0; i < match.Groups["property"].Captures.Count; i++)
                parameters.Add(match.Groups["name"].Captures[i].Value.ToLower(), match.Groups["value"].Captures[i].Value);

            var macroAlias = parameters["macroalias"];
            parameters.Remove("macroalias");

            var macro = new Macro(macroAlias);
            string filepath = "~/" + macro.Type;

            var page = (System.Web.UI.Page) HttpContext.Current.Handler;
            var control = page.LoadControl(filepath);

            var type = control.GetType();

            var controlProperties = type.GetProperties().ToDictionary(p => p.Name.ToLower());

            foreach (var key in parameters.Keys)
            {
                if (!controlProperties.ContainsKey(key.ToLower()))
                    continue;

                var propertyInfo = controlProperties[key.ToLower()];
                if (propertyInfo == null || !propertyInfo.CanWrite) 
                    continue;
                var converter = TypeDescriptor.GetConverter(propertyInfo.PropertyType);
                if (converter == null) 
                    continue;
                var value = converter.ConvertFrom(parameters[key]);
                propertyInfo.SetValue(control, value, null);
            }
            return control;
        }

        private static string Capitalize(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
            return Char.ToUpper(text[0]) + text.Substring(1);
        }

        private static Control LoadModule(Match match, int? columnSpan)
        {
            if (match == null)
                return null;

            return LoadModule(match.Groups["ref"].Value, columnSpan);
        }

        public static Control LoadModule(string moduleId, int? columnSpan)
        {
            if (string.IsNullOrEmpty(moduleId))
                throw new ArgumentNullException("moduleId");

            var id = new Id(moduleId);
            var module = CmsService.Instance.GetItem<Entity>(id);
            var modulePath = string.Format("~/modules/{0}Rendering.ascx", module.Template.Name);

            var page = (System.Web.UI.Page)HttpContext.Current.Handler;

            var control = (IGridModuleRendering)page.LoadControl(modulePath);
            control.InitializeModule(id.ToString(), columnSpan);
            return (Control)control;
        }
    }
}
