using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using umbraco;
using umbraco.BusinessLogic;

namespace LinqIt.UmbracoCustomFieldTypes.Utilities
{
    public static class UmbracoRichTextHelper
    {
        public static string ConvertMacrosToHtml(string input, int pageId, Guid versionId)
        {
            var result = input;
            const string pattern = @"(<\?UMBRACO_MACRO\W*[^>]*/>)";
            var matches = Regex.Matches(result, pattern, RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);
            HttpContext.Current.Items["macrosAdded"] = 0;
            HttpContext.Current.Items["pageID"] = pageId.ToString();
            foreach (Match match in matches)
            {
                try
                {
                    umbraco.macro macro;
                    var attributes = helper.ReturnAttributes(match.Groups[1].Value);
                    var newValue = umbraco.macro.renderMacroStartTag(attributes, pageId, versionId);
                    if (helper.FindAttribute(attributes, "macroID") != "")
                    {
                        macro = umbraco.macro.GetMacro(int.Parse(helper.FindAttribute(attributes, "macroID")));
                    }
                    else
                    {
                        var macroAlias = helper.FindAttribute(attributes, "macroAlias");
                        if (macroAlias == "")
                        {
                            macroAlias = helper.FindAttribute(attributes, "macroalias");
                            attributes.Remove("macroalias");
                            attributes.Add("macroAlias", macroAlias);
                        }
                        if (macroAlias == "")
                        {
                            throw new ArgumentException("umbraco is unable to identify the macro. No id or macroalias was provided for the macro in the macro tag.", match.Groups[1].Value);
                        }
                        macro = umbraco.macro.GetMacro(macroAlias);
                    }
                    if (helper.FindAttribute(attributes, "macroAlias") == "")
                    {
                        attributes.Add("macroAlias", macro.Alias);
                    }
                    try
                    {
                        newValue = newValue + umbraco.macro.MacroContentByHttp(pageId, versionId, attributes);
                    }
                    catch
                    {
                        newValue = newValue + "<span style=\"color: green\">No macro content available for WYSIWYG editing</span>";
                    }
                    newValue = newValue + umbraco.macro.renderMacroEndTag();
                    result = result.Replace(match.Groups[1].Value, newValue);
                    continue;
                }
                catch (Exception exception)
                {
                    Log.Add(LogTypes.Error, pageId, "Macro Parsing Error: " + exception.ToString());
                    const string errorTag = "<div class=\"umbMacroHolder mceNonEditable\"><p style=\"color: red\"><strong>umbraco was unable to parse a macro tag, which means that parts of this content might be corrupt.</strong> <br /><br />Best solution is to rollback to a previous version by right clicking the node in the tree and then try to insert the macro again. <br/><br/>Please report this to your system administrator as well - this error has been logged.</p></div>";
                    result = result.Replace(match.Groups[1].Value, errorTag);
                    continue;
                }
            }
            return result;
        }

        public static string ConvertHtmlToMacros(string text)
        {
            while (findStartTag(text) > -1)
            {
                string oldValue = text.Substring(findStartTag(text), findEndTag(text) - findStartTag(text));
                text = text.Replace(oldValue, generateMacroTag(oldValue));
            }
            return text;
        }

        private static string generateMacroTag(string macroContent)
        {
            var tag = macroContent.Substring(5, macroContent.IndexOf(">") - 5);
            var str2 = "<?UMBRACO_MACRO ";
            var enumerator = ReturnAttributes(tag).GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Key.ToString().IndexOf("umb_") != -1)
                {
                    string str3 = enumerator.Key.ToString();
                    if (str3 == "umb_macroalias")
                    {
                        str3 = "umb_macroAlias";
                    }
                    string str4 = str2;
                    str2 = str4 + str3.Substring(4, str3.Length - 4) + "=\"" + enumerator.Value.ToString().Replace(@"\r\n", Environment.NewLine) + "\" ";
                }
            }
            return (str2 + "/>");
        }

        public static Hashtable ReturnAttributes(string tag)
        {
            var hashtable = new Hashtable();
            foreach (Match match in Regex.Matches(tag, "(?<attributeName>\\S*)=\"(?<attributeValue>[^\"]*)\"", RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase))
            {
                hashtable.Add(match.Groups["attributeName"].Value, match.Groups["attributeValue"].Value);
            }
            return hashtable;
        }



        private static int findEndTag(string text)
        {
            const string str = "<!-- endumbmacro -->";
            var index = text.ToLower().IndexOf(str);
            var str2 = text.ToLower().Substring(index + str.Length, (text.Length - index) - str.Length);
            var startIndex = 0;
            while (str2.Length > 5)
            {
                if (str2.Substring(startIndex, 6) == "</div>")
                {
                    break;
                }
                startIndex++;
            }
            return (((index + str.Length) + startIndex) + 6);
        }

        private static int findStartTag(string text)
        {
            text = text.ToLower();
            if (text.IndexOf("ismacro=\"true\"") > -1)
            {
                return text.Substring(0, text.IndexOf("ismacro=\"true\"")).LastIndexOf("<");
            }
            return -1;
        }
    }
}
