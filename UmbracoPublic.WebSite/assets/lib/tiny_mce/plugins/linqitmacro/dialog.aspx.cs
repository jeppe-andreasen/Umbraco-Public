using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Parsing.Html;
using LinqIt.Utils.Collections;
using umbraco.cms.businesslogic.macro;
using umbraco.interfaces;
using umbraco.presentation;
using umbraco.uicontrols;
using LinqIt.Utils.Web;

namespace UmbracoPublic.WebSite.assets.lib.tiny_mce.plugins.linqitmacro
{
    public partial class dialog : System.Web.UI.Page
    {
        private readonly List<Control> _dataFields = new List<Control>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request.Form["uiInitialized"]))
                uiInitialized.Value = "false";
            else if (Request.Form["uiInitialized"] == "false")
            {
                var selection = Request.Form["uiSelection"];
                InitializeForm(selection);
                uiInitialized.Value = "true";
            }
            else if (mvItems.ActiveViewIndex == 1)
            {
                BuildEditorForm();
            }
        }

        protected string MacroAlias
        {
            get { return (string)ViewState["MacroAlias"]; }
            set { ViewState["MacroAlias"] = value; }
        }

        private void InitializeForm(string selection)
        {
            if (!string.IsNullOrEmpty(selection))
            {
                var doc = new HtmlDocument(selection);
                var root = doc.FindElements(t => t.Attributes["class"] == "umbMacroHolder" && t.Attributes["ismacro"] == "true").FirstOrDefault();
                if (root != null)
                {
                    MacroAlias = root.Attributes["umb_macroalias"];
                    BuildEditorForm(root.Attributes);
                    mvItems.ActiveViewIndex = 1;
                    return;
                }
            }

            ddlMacroType.Items.Add(new ListItem("", ""));
            foreach (var macro in Macro.GetAll())
            {
                if (!macro.UseInEditor)
                    continue;

                ddlMacroType.Items.Add(new ListItem(macro.Name, macro.Alias));
            }
            mvItems.ActiveViewIndex = 0;
        }

        protected void OnMacroTypeSelected(object sender, EventArgs e)
        {
            MacroAlias = ddlMacroType.SelectedValue;
            BuildEditorForm();
            mvItems.ActiveViewIndex = 1;
        }

        private void BuildEditorForm(CaseInvariantNameValueCollection attributes = null)
        {
            plhEditor.Controls.Clear();
            if (string.IsNullOrEmpty(MacroAlias))
                return;
            var macro = Macro.GetByAlias(MacroAlias);
            foreach (var property in macro.Properties)
            {
                try
                {
                    var type = Type.GetType(property.Type.Assembly + "." + property.Type.Type + "," + property.Type.Assembly);
                    var control = Activator.CreateInstance(type) as Control;
                    if (control != null && control is IMacroGuiRendering)
                    {
                        control.ID = property.Alias;
                        if (attributes != null)
                        {
                            var propertyValue = attributes["umb_" + property.Alias];
                            if (propertyValue != null)
                            {
                                propertyValue = HttpUtility.UrlDecode(propertyValue.Replace(@"\r", "\r").Replace(@"\n", "\n").Replace("\\\"", "\""));
                                if (propertyValue != "")
                                    type.GetProperty("Value").SetValue(control, Convert.ChangeType(propertyValue, type.GetProperty("Value").PropertyType), null);
                            }
                        }
                        var panel = new PropertyPanel { Text = property.Name };
                        panel.Controls.Add(control);
                        //this._scriptOnLoad = this._scriptOnLoad + "\t\tregisterAlias('" + control.ID + "');\n";
                        plhEditor.Controls.Add(panel);
                        _dataFields.Add(control);
                    }
                    else
                    {
                        Trace.Warn("umbEditContent", "Type doesn't exist or is not umbraco.interfaces.DataFieldI ('" + property.Type.Assembly + "." + property.Type.Type + "')");
                    }
                }
                catch (Exception exception)
                {
                    Trace.Warn("umbEditContent", "Error creating type '" + property.Type.Assembly + "." + property.Type.Type + "'", exception);
                }
            }
        }

        protected void OnOkClicked(object sender, EventArgs e)
        {
            uiValue.Value = HtmlWriter.Generate(RenderOutput);
            ClientScript.RegisterStartupScript(GetType(), "submission", "submitValue();", true);
        }

        private void RenderOutput(HtmlWriter writer)
        {
            foreach (var control in _dataFields)
                writer.AddAttribute("umb_" + control.ID, ((IMacroGuiRendering)control).Value);    
            writer.AddAttribute("umb_macroalias", MacroAlias);
            writer.AddAttribute("ismacro", "true");
            writer.AddAttribute("onresizestart", "return false;");
            writer.AddAttribute("umbversionid", "cc9ae04b-c494-4692-a4b9-c20416b050b1");
            writer.AddAttribute("umbpageid", Request.QueryString["id"]);
            writer.AddAttribute(HtmlTextWriterAttribute.Title, "This is rendered content from macro");
            writer.RenderBeginTag(HtmlTextWriterTag.Div, "umbMacroHolder");
            writer.AddAttribute(HtmlTextWriterAttribute.Style, "color: green;");
            writer.RenderBeginTag(HtmlTextWriterTag.Span);
            writer.Write("<!-- startUmbMacro --><span style=\"color: green;\">");
            writer.RenderFullTag(HtmlTextWriterTag.Strong, "Block Quote Macro");
            writer.WriteBreak();
            writer.Write("No macro content available for WYSIWYG editing");
            writer.RenderEndTag(); // span
            writer.Write("<!-- endUmbMacro -->");
            writer.RenderEndTag(); // div.umbMacroHolder
        }
    }
}