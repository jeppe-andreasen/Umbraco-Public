using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Ajax.Parsing;
using LinqIt.Utils.Extensions;
using umbraco.cms.businesslogic.web;

namespace LinqIt.UmbracoCustomFieldTypes.Components
{
    public class TinyMCEditor : Control, INamingContainer
    {
        private TextBox _textbox;
        private bool _displayMacroPlugin = true;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!Page.ClientScript.IsClientScriptIncludeRegistered(Page.GetType(), "tinyMCEditSrc"))
                Page.ClientScript.RegisterClientScriptInclude(Page.GetType(), "tinyMCEditSrc", "/assets/lib/tiny_mce/tiny_mce.js");
        }

        public string HiddenId { get; set; }

        public bool RegisterFormBind { get; set; }

        public Unit? Width
        {
            get
            {
                EnsureChildControls();
                return _textbox.Width;
            }
            set
            {
                EnsureChildControls();
                if (value.HasValue)
                    _textbox.Width = value.Value;
            }
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            _textbox = new TextBox();
            _textbox.TextMode = TextBoxMode.MultiLine;
            _textbox.CssClass = "tinymceditor";
            Controls.Add(_textbox);
        }

        protected override void OnPreRender(EventArgs e)
        {
            var options = new JSONObject();
            options.AddValue("mode", "exact");
            if (Width.HasValue)
                options.AddValue("width", Width.Value.Value.ToString());
            else
                options.AddValue("width", 800);
            options.AddValue("elements", _textbox.ClientID);
            options.AddValue("theme", "advanced");
            options.AddValue("plugins", GetPlugins());

            var styleOptions = GetStyleOptions();
            if (!string.IsNullOrEmpty(styleOptions))
                options.AddValue("theme_advanced_styles", styleOptions);
            options.AddValue("theme_advanced_buttons1", GetButtons1());
            options.AddValue("theme_advanced_buttons2", GetButtons2());
            options.AddValue("theme_advanced_buttons3", GetButtons3());
            options.AddValue("theme_advanced_toolbar_location", "top");
            options.AddValue("theme_advanced_toolbar_align", "left");
            options.AddValue("theme_advanced_statusbar_location", "bottom");
            options.AddValue("template_external_list_url", "lists/template_list.js");
            options.AddValue("external_link_list_url", "lists/link_list.js");
            options.AddValue("external_image_list_url", "lists/image_list.js");
            options.AddValue("media_external_list_url", "lists/media_list.js");

            if (StyleSheets != null && StyleSheets.Any())
                options.AddValue("content_css", StyleSheets.ToSeparatedString(","));
            
            options.AddValue("verify_html", false);
            options.AddValue("valid_elements", "*[*]");
            //options.AddValue("valid_elements", GetValidElements());
            options.AddValue("invalid_elements", GetInvalidElements());




            //options.AddValue("template_replace_values", JSONObject.Parse("{ username : \"Some User\", staffid : \"991234\" }"));

            var setup = new JSONDelegate("ed");
            if (!string.IsNullOrEmpty(HiddenId))
            {
                
                setup.Lines.Add("ed.onInit.add(function(ed, evt) {");
                setup.Lines.Add("initializeEditorValue(ed, doc, '" + HiddenId + "');");
                setup.Lines.Add("var dom = ed.dom;");
                setup.Lines.Add("var doc = ed.getDoc();");
                setup.Lines.Add("tinymce.dom.Event.add(doc, 'blur', function(e) {");
                setup.Lines.Add("updateEditorValue(ed, doc,'" + HiddenId + "');");
                setup.Lines.Add("});");
                setup.Lines.Add("});");
            }

            setup.Lines.Add("ed.onNodeChange.addToTop(function(ed, cm, n) {");
            setup.Lines.Add("var macroElement = ed.dom.getParent(ed.selection.getStart(), 'div.umbMacroHolder');");
            setup.Lines.Add("if (macroElement) {");
            setup.Lines.Add("ed.selection.select(macroElement);");
            setup.Lines.Add("var currentSelection = ed.selection.getStart();");
            setup.Lines.Add("if (tinymce.isIE) {");
            setup.Lines.Add("if (!ed.dom.hasClass(currentSelection, 'umbMacroHolder')) {");
            setup.Lines.Add("while (!ed.dom.hasClass(currentSelection, 'umbMacroHolder') && currentSelection.parentNode) {");
            setup.Lines.Add("currentSelection = currentSelection.parentNode;");
            setup.Lines.Add("}");
            setup.Lines.Add("ed.selection.select(currentSelection);");
            setup.Lines.Add("}");
            setup.Lines.Add("}");
            setup.Lines.Add("cm.setActive('umbracomacro', ed.dom.hasClass(currentSelection, 'umbMacroHolder') || ed.dom.hasClass(macroElement, 'umbMacroHolder'));");
            setup.Lines.Add("}");
            setup.Lines.Add("});");
            
            options.AddValue("setup", setup);

            var script = new StringBuilder();
            script.AppendLine("tinyMCE.init(" + options.ToString() + ");");

            if (RegisterFormBind)
                script.AppendLine("$(theForm).bind(\"onSave\", function() { $(\"#" + _textbox.ClientID + "\").val(tinyMCE.get('" + _textbox.ClientID + "').save()); });");

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "tinyMCEditor" + _textbox.ClientID, script.ToString(), true);

            base.OnPreRender(e);
        }

        private static string GetInvalidElements()
        {
            var elements = new List<string>();
            elements.Add("font");
            elements.Add("pre");
            return elements.ToSeparatedString(",");
        }

        private static string GetValidElements()
        {
            var elements = new List<string>();
            elements.Add("+a[id|style|rel|rev|charset|hreflang|dir|lang|tabindex|accesskey|type|name|href|target|title|class|onfocus|onblur|onclick|ondblclick|onmousedown|onmouseup|onmouseover|onmousemove|onmouseout|onkeypress|onkeydown|onkeyup]");
            elements.Add("-strong/-b[class|style]");
            elements.Add("-em/-i[class|style]");
            elements.Add("-strike[class|style]");
            elements.Add("-u[class|style]");
            elements.Add("#p[id|style|dir|class|align]");
            elements.Add("-ol[class|style]");
            elements.Add("-ul[class|style]");
            elements.Add("-li[class|style]");
            elements.Add("br");
            elements.Add("img[id|dir|lang|longdesc|usemap|style|class|src|onmouseover|onmouseout|border|alt=|title|hspace|vspace|width|height|align|umbracoorgwidth|umbracoorgheight|onresize|onresizestart|onresizeend|rel]");
            elements.Add("-sub[style|class]");
            elements.Add("-sup[style|class]");
            elements.Add("-blockquote[dir|style]");
            elements.Add("-table[border=0|cellspacing|cellpadding|width|height|class|align|summary|style|dir|id|lang|bgcolor|background|bordercolor]");
            elements.Add("-tr[id|lang|dir|class|rowspan|width|height|align|valign|style|bgcolor|background|bordercolor]");
            elements.Add("tbody[id|class]");
            elements.Add("thead[id|class]");
            elements.Add("tfoot[id|class]");
            elements.Add("-td[id|lang|dir|class|colspan|rowspan|width|height|align|valign|style|bgcolor|background|bordercolor|scope]");
            elements.Add("-th[id|lang|dir|class|colspan|rowspan|width|height|align|valign|style|scope]");
            elements.Add("caption[id|lang|dir|class|style]");
            elements.Add("-div[id|dir|class|align|style]");
            elements.Add("-span[class|align|style]");
            elements.Add("-pre[class|align|style]");
            elements.Add("address[class|align|style]");
            elements.Add("-h1[id|dir|class|align]");
            elements.Add("-h2[id|dir|class|align]");
            elements.Add("-h3[id|dir|class|align]");
            elements.Add("-h4[id|dir|class|align]");
            elements.Add("-h5[id|dir|class|align]");
            elements.Add("-h6[id|style|dir|class|align]");
            elements.Add("hr[class|style]");
            elements.Add("dd[id|class|title|style|dir|lang]");
            elements.Add("dl[id|class|title|style|dir|lang]");
            elements.Add("dt[id|class|title|style|dir|lang]");
            elements.Add("object[classid|width|height|codebase|*]");
            elements.Add("param[name|value|_value]");
            elements.Add("embed[type|width|height|src|*]");
            elements.Add("map[name]");
            elements.Add("area[shape|coords|href|alt|target]");
            elements.Add("bdo");
            elements.Add("button");
            elements.Add("iframe[*]");
            return elements.ToSeparatedString(",");
        }

        private string GetStyleOptions()
        {
            var stylesheet = !string.IsNullOrEmpty(StyleDefinitionSheet) ? umbraco.cms.businesslogic.web.StyleSheet.GetByName(StyleDefinitionSheet) : null;
            if (stylesheet == null || !stylesheet.Properties.Any())
                return null;
            return stylesheet.Properties.ToSeparatedString(";", p => string.Format("{0}={1}", p.Text, p.Alias.TrimStart('.')));
        }

        private string GetPlugins()
        {
            var result = new List<string>();
            result.Add("linqitlink");
            if (_displayMacroPlugin)
                result.Add("linqitmacro");
            result.Add("autolink");
            result.Add("lists");
            result.Add("pagebreak");
            result.Add("style");
            result.Add("layer");
            result.Add("table");
            result.Add("save");
            result.Add("advhr");
            result.Add("advimage");
            result.Add("emotions");
            result.Add("iespell");
            result.Add("inlinepopups");
            result.Add("insertdatetime");
            result.Add("preview");
            result.Add("media");
            result.Add("searchreplace");
            result.Add("print");
            result.Add("contextmenu");
            result.Add("paste");
            result.Add("directionality");
            result.Add("fullscreen");
            result.Add("noneditable");
            result.Add("visualchars");
            result.Add("nonbreaking");
            result.Add("xhtmlxtras");
            result.Add("template");
            result.Add("wordcount");
            result.Add("advlist");
            result.Add("autosave");
            result.Add("visualblocks");
            return result.ToSeparatedString(",");
        }

        private static IEnumerable<string> GetButtons1List()
        {
            var result = new List<string>();
            result.Add("bold");
            result.Add("italic");
            result.Add("underline");
            result.Add("strikethrough");
            result.Add("|");
            result.Add("justifyleft");
            result.Add("justifycenter");
            result.Add("justifyright");
            result.Add("justifyfull");
            result.Add("styleselect");
            result.Add("formatselect");
            result.Add("removeformat");
            result.Add("fullscreen");
            //result.Add("fontselect");
            //result.Add("fontsizeselect");
            return result;
        }

        private static string GetButtons1()
        {
            return GetButtons1List().ToSeparatedString(",");
        }

        private IEnumerable<string> GetButtons2List()
        {
            var result = new List<string>();
            result.Add("cut");
            result.Add("copy");
            result.Add("paste");
            result.Add("pastetext");
            result.Add("pasteword");
            result.Add("|");
            result.Add("search");
            result.Add("replace");
            result.Add("|");
            result.Add("bullist");
            result.Add("numlist");
            //result.Add("|");
            //result.Add("outdent");
            //result.Add("indent");
            //result.Add("blockquote");
            result.Add("|");
            result.Add("undo");
            result.Add("redo");
            result.Add("|");
            result.Add("linqitlink");
            result.Add("unlink");
            result.Add("anchor");
            result.Add("image");
            //result.Add("cleanup");
            result.Add("help");
            result.Add("code");
            if (_displayMacroPlugin)
                result.Add("linqitmacro");
            //result.Add("|");
            //result.Add("insertdate");
            //result.Add("inserttime");
            //result.Add("preview");
            //result.Add("|");
            //result.Add("forecolor");
            //result.Add("backcolor");
            return result;
        }

        private string GetButtons2()
        {
            return GetButtons2List().ToSeparatedString(",");
        }

        private static IEnumerable<string> GetButtons3List()
        {
            var result = new List<string>();
            result.Add("tablecontrols");
            result.Add("|");
            result.Add("hr");
            
            //result.Add("visualaid");
            //result.Add("|");
            //result.Add("sub");
            //result.Add("sup");
            //result.Add("|");
            //result.Add("charmap");
            //result.Add("emotions");
            //result.Add("iespell");
            //result.Add("media");
            //result.Add("advhr");
            //result.Add("|");
            //result.Add("print");
            //result.Add("|");
            //result.Add("ltr");
            //result.Add("rtl");
            //result.Add("|");
           
            return result;
            
        }

        private static string GetButtons3()
        {
            
            return GetButtons3List().ToSeparatedString(",");
        }

        private static IEnumerable<string> GetButtons4List()
        {
            var result = new List<string>();
            result.Add("insertlayer");
            result.Add("moveforward");
            result.Add("movebackward");
            result.Add("absolute");
            result.Add("|");
            result.Add("styleprops");
            result.Add("|");
            result.Add("cite");
            result.Add("abbr");
            result.Add("acronym");
            result.Add("del");
            result.Add("ins");
            result.Add("attribs");
            result.Add("|");
            result.Add("visualchars");
            result.Add("nonbreaking");
            result.Add("template");
            result.Add("pagebreak");
            result.Add("restoredraft");
            result.Add("visualblocks");
            return result;
        }

        private static string GetButtons4()
        {
            return GetButtons4List().ToSeparatedString(",");
        }

        public string Value
        {
            get
            {
                EnsureChildControls();
                return _textbox.Text;
            }
            set
            {
                EnsureChildControls();
                _textbox.Text = value;
            }
        }

        public string StyleDefinitionSheet { get; set; }

        public string[] StyleSheets { get; set; }

        public bool DisplayMacroPlugin
        {
            get { return _displayMacroPlugin; }
            set { _displayMacroPlugin = value; }
        }
    }
}
