//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Threading;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using umbraco;
//using umbraco.BasePages;
//using umbraco.cms.businesslogic;
//using umbraco.cms.businesslogic.property;
//using umbraco.cms.businesslogic.propertytype;
//using umbraco.cms.businesslogic.web;
//using umbraco.controls;
//using umbraco.interfaces;
//using umbraco.IO;
//using umbraco.uicontrols;
//using Content = umbraco.cms.businesslogic.Content;

//namespace UmbracoPublic.WebSite.umbraco
//{
//    public sealed class CustomContentControl : TabView
//    {
//        // Fields
//        private readonly Content _content;
//        private readonly ArrayList _dataFields = new ArrayList();
//        private string _errorMessage = "";
//        private static readonly string _umbracoPath = SystemDirectories.Umbraco;
//        private static AfterContentControlLoadEventHandler _afterContenControlLoad;
//        private static BeforeContentControlLoadEventHandler _beforeContentControlLoad;
//        private readonly PublishModes _canPublish = PublishModes.NoPublish;
//        public bool DoesPublish;
//        public TextBox _txtName = new TextBox();
//        public PlaceHolder _phName = new PlaceHolder();
//        public RequiredFieldValidator _rfvName = new RequiredFieldValidator();
//        private UmbracoEnsuredPage _printPage;
//        public Pane PropertiesPane = new Pane();
//        private EventHandler _onSave;
//        private EventHandler _onSaveAndPublish;
//        private EventHandler _onSaveToPublish;
//        public TabPage PropertiesTab;
//        private List<ContentType.TabI> _virtualTabs;

        

//        // Events
//        public static event AfterContentControlLoadEventHandler AfterContentControlLoad
//        {
//            add
//            {
//                AddHandler(value, _afterContenControlLoad);
//            }
//            remove
//            {
//                RemoveHandler(value, _afterContenControlLoad);
//            }
//        }

//        private static void AddHandler(Delegate value, Delegate staticHandler)
//        {
//            Delegate handler2;
//            var tmp = staticHandler;
//            do
//            {
//                handler2 = tmp;
//                var handler3 = Delegate.Combine(handler2, value);
//                tmp = Interlocked.CompareExchange(ref staticHandler, handler3, handler2);
//            }
//            while (tmp != handler2);
//        }

//        private static void RemoveHandler(Delegate value, Delegate staticHandler)
//        {
//            Delegate handler2;
//            var tmp = staticHandler;
//            do
//            {
//                handler2 = tmp;
//                var handler3 = Delegate.Remove(handler2, value);
//                tmp = Interlocked.CompareExchange(ref staticHandler, handler3, handler2);
//            }
//            while (tmp != handler2);
//        }

//        public static event BeforeContentControlLoadEventHandler BeforeContentControlLoad
//        {
//            add
//            {
//                AddHandler(value, _beforeContentControlLoad);
//            }
//            remove
//            {
//                RemoveHandler(value, _beforeContentControlLoad);
//            }
//        }

//        public event EventHandler Save
//        {
//            add
//            {
//                AddHandler(value, _onSave);
//            }
//            remove
//            {
//                RemoveHandler(value, _onSave);
//            }
//        }

//        public event EventHandler SaveAndPublish
//        {
//            add
//            {
//                AddHandler(value, _onSaveAndPublish);
//            }
//            remove
//            {
//                RemoveHandler(value, _onSaveAndPublish);
//            }
//        }

//        public event EventHandler SaveToPublish
//        {
//            add
//            {
//                AddHandler(value, _onSaveToPublish);
//            }
//            remove
//            {
//                RemoveHandler(value, _onSaveToPublish);
//            }
//        }

//        // Methods
//        public CustomContentControl(Content c, PublishModes CanPublish, string Id)
//        {
//            ID = Id;
//            _canPublish = CanPublish;
//            _content = c;
//            Width = 350;
//            Height = 350;
//            SaveAndPublish += EmptyHandler;
//            Save += EmptyHandler;
//            _printPage = (UmbracoEnsuredPage)Page;
//            if (_virtualTabs == null)
//                _virtualTabs = _content.ContentType.getVirtualTabs.ToList();
//            foreach (var virtualTab in _virtualTabs)
//            {
//                var tabPage = NewTabPage(virtualTab.Caption);
//                addSaveAndPublishButtons(ref tabPage);
//            }
//        }

//        private void addControlNew(Property p, TabPage tp, string caption)
//        {
//            var dataType = p.PropertyType.DataTypeDefinition.DataType;
//            dataType.DataEditor.Editor.ID = string.Format("prop_{0}", p.PropertyType.Alias);
//            dataType.Data.PropertyId = p.Id;
//            var editor = dataType.DataEditor.Editor as IDataFieldWithButtons;
            
//            if (editor != null)
//            {
//                ((Control)editor).ID = p.PropertyType.Alias;
//                if (editor.MenuIcons.Length > 0)
//                {
//                    tp.Menu.InsertSplitter();
//                }
//                var flag = false;
//                foreach (var menuIcon in editor.MenuIcons)
//                {
//                    bool flag2;
//                    try
//                    {
//                        var ni = (MenuIconI)menuIcon;
//                        var ni2 = tp.Menu.NewIcon();
//                        ni2.ImageURL = ni.ImageURL;
//                        ni2.OnClickCommand = ni.OnClickCommand;
//                        ni2.AltText = ni.AltText;
//                        ni2.ID = tp.ID + "_" + ni.ID;
//                        flag = ni.ID == "html";
//                        flag2 = false;
//                    }
//                    catch
//                    {
//                        tp.Menu.InsertSplitter();
//                        flag2 = true;
//                    }
//                    if ((flag2 && flag) && dataType.DataEditor.TreatAsRichTextEditor)
//                    {
//                        var list = tp.Menu.NewDropDownList();
//                        list.Style.Add("margin-bottom", "5px");
//                        list.Items.Add(ui.Text("buttons", "styleChoose", null));
//                        list.ID = tp.ID + "_editorStyle";
//                        if (StyleSheet.GetAll().Length > 0)
//                        {
//                            foreach (StylesheetProperty property in StyleSheet.GetAll().SelectMany(sheet => sheet.Properties))
//                            {
//                                list.Items.Add(new ListItem(property.Text, property.Alias));
//                            }
//                        }
//                        list.Attributes.Add("onChange", "addStyle(this, '" + p.PropertyType.Alias + "');");
//                        flag = false;
//                    }
//                }
//            }
//            var element = dataType.DataEditor.Editor as IMenuElement;
//            if (element != null)
//            {
//                tp.Menu.InsertSplitter();
//                tp.Menu.NewElement(element.ElementName, element.ElementIdPreFix + p.Id.ToString(), element.ElementClass, element.ExtraMenuWidth);
//            }
//            this._dataFields.Add(dataType.DataEditor.Editor);
//            var child = new Pane();
//            var ctrl = new Control();
//            ctrl.Controls.Add(dataType.DataEditor.Editor);
//            if (!p.PropertyType.DataTypeDefinition.DataType.DataEditor.ShowLabel)
//            {
//                child.addProperty(ctrl);
//            }
//            else
//            {
//                string str2;
//                var name = p.PropertyType.Name;
//                if (((!string.IsNullOrEmpty(p.PropertyType.Description))) && ((str2 = UmbracoSettings.PropertyContextHelpOption) != null))
//                {
//                    if (str2 != "icon")
//                    {
//                        if (str2 == "text")
//                        {
//                            name = name + "<br /><small>" + library.ReplaceLineBreaks(p.PropertyType.Description) + "</small>";
//                        }
//                    }
//                    else
//                    {
//                        string str3 = name;
//                        name = str3 + " <img src=\"" + base.ResolveUrl(SystemDirectories.Umbraco) + "/images/help.png\" class=\"umbPropertyContextHelp\" alt=\"" + p.PropertyType.Description + "\" title=\"" + p.PropertyType.Description + "\" />";
//                    }
//                }
//                child.addProperty(name, ctrl);
//            }
//            if (p.PropertyType.Mandatory)
//            {
//                try
//                {
//                    var validator = new RequiredFieldValidator
//                    {
//                        ControlToValidate = dataType.DataEditor.Editor.ID
//                    };
//                    var component = dataType.DataEditor.Editor;
//                    var attribute = (ValidationPropertyAttribute)TypeDescriptor.GetAttributes(component)[typeof(ValidationPropertyAttribute)];
//                    PropertyDescriptor descriptor = null;
//                    if (attribute != null)
//                    {
//                        descriptor = TypeDescriptor.GetProperties(component, (Attribute[])null)[attribute.Name];
//                    }
//                    if (descriptor != null)
//                    {
//                        validator.EnableClientScript = false;
//                        validator.Display = ValidatorDisplay.Dynamic;
//                        var variables = new[] { p.PropertyType.Name, caption };
//                        validator.ErrorMessage = ui.Text("errorHandling", "errorMandatory", variables, null) + "<br/>";
//                        ctrl.Controls.AddAt(0, validator);
//                    }
//                }
//                catch (Exception exception)
//                {
//                    HttpContext.Current.Trace.Warn("contentControl", "EditorControl (" + dataType.DataTypeName + ") does not support validation", exception);
//                }
//            }
//            if (p.PropertyType.ValidationRegExp != "")
//            {
//                try
//                {
//                    var validator2 = new RegularExpressionValidator
//                    {
//                        ControlToValidate = dataType.DataEditor.Editor.ID
//                    };
//                    var control3 = dataType.DataEditor.Editor;
//                    var attribute2 = (ValidationPropertyAttribute)TypeDescriptor.GetAttributes(control3)[typeof(ValidationPropertyAttribute)];
//                    PropertyDescriptor descriptor2 = null;
//                    if (attribute2 != null)
//                    {
//                        descriptor2 = TypeDescriptor.GetProperties(control3, (Attribute[])null)[attribute2.Name];
//                    }
//                    if (descriptor2 != null)
//                    {
//                        validator2.ValidationExpression = p.PropertyType.ValidationRegExp;
//                        validator2.EnableClientScript = false;
//                        validator2.Display = ValidatorDisplay.Dynamic;
//                        var strArray2 = new[] { p.PropertyType.Name, caption };
//                        validator2.ErrorMessage = ui.Text("errorHandling", "errorRegExp", strArray2, null) + "<br/>";
//                        ctrl.Controls.AddAt(0, validator2);
//                    }
//                }
//                catch (Exception exception2)
//                {
//                    HttpContext.Current.Trace.Warn("contentControl", "EditorControl (" + dataType.DataTypeName + ") does not support validation", exception2);
//                }
//            }
//            if (dataType.DataEditor.TreatAsRichTextEditor)
//            {
//                tp.Controls.Add(dataType.DataEditor.Editor);
//            }
//            else
//            {
//                var panel = new Panel();
//                panel.Attributes.Add("style", "padding: 0; position: relative;");
//                panel.Controls.Add(child);
//                tp.Controls.Add(panel);
//            }
//        }

//        private void addSaveAndPublishButtons(ref TabPage tp)
//        {
//            var button = tp.Menu.NewImageButton();
//            button.ID = tp.ID + "_save";
//            button.ImageUrl = _umbracoPath + "/images/editor/save.gif";
//            button.Click += OnSaveClicked;
//            button.OnClickCommand = "invokeSaveHandlers();";
//            button.AltText = ui.Text("buttons", "save", null);
//            if (_canPublish == PublishModes.Publish)
//            {
//                var button2 = tp.Menu.NewImageButton();
//                button2.ID = tp.ID + "_publish";
//                button2.ImageUrl = _umbracoPath + "/images/editor/saveAndPublish.gif";
//                button2.OnClickCommand = "invokeSaveHandlers();";
//                button2.Click += OnSaveAndPublishClicked;
//                button2.AltText = ui.Text("buttons", "saveAndPublish", null);
//            }
//            else if (_canPublish == PublishModes.SendToPublish)
//            {
//                var button3 = tp.Menu.NewImageButton();
//                button3.ID = tp.ID + "_topublish";
//                button3.ImageUrl = _umbracoPath + "/images/editor/saveToPublish.gif";
//                button3.OnClickCommand = "invokeSaveHandlers();";
//                button3.Click += saveToPublish;
//                button3.AltText = ui.Text("buttons", "saveToPublish", null);
//            }
//        }

//        protected override void CreateChildControls()
//        {
//            base.CreateChildControls();
//            this.SaveAndPublish += EmptyHandler;
//            this.Save += EmptyHandler;
//            this._printPage = (UmbracoEnsuredPage)Page;
//            int num = 0;
//            var hashtable = new Hashtable();
//            if (this._virtualTabs == null)
//            {
//                this._virtualTabs = this._content.ContentType.getVirtualTabs.ToList<ContentType.TabI>();
//            }
//            foreach (ContentType.TabI bi in this._virtualTabs)
//            {
//                TabPage tp = base.Panels[num] as TabPage;
//                if (tp == null)
//                {
//                    throw new ArgumentException("Unable to load tab \"" + bi.Caption + "\"");
//                }
//                tp.Style.Add("text-align", "center");
//                foreach (PropertyType type in bi.GetPropertyTypes(this._content.ContentType.Id))
//                {
//                    Property p = this._content.getProperty(type);
//                    if (p == null)
//                    {
//                        throw new ArgumentNullException(string.Format("Property {0} ({1}) on Content Type {2} could not be retrieved for Document {3} on Tab Page {4}. To fix this problem, delete the property and recreate it.", new object[] { type.Alias, type.Id, this._content.ContentType.Alias, this._content.Id, bi.Caption }));
//                    }
//                    this.addControlNew(p, tp, bi.Caption);
//                    if (!hashtable.ContainsKey(type.Id.ToString()))
//                    {
//                        hashtable.Add(type.Id.ToString(), true);
//                    }
//                }
//                num++;
//            }
//            this.PropertiesTab = base.NewTabPage(ui.Text("general", "properties", null));
//            this.addSaveAndPublishButtons(ref this.PropertiesTab);
//            this.PropertiesTab.Controls.Add(new LiteralControl("<div id=\"errorPane_" + this.PropertiesTab.ClientID + "\" style=\"display: none; text-align: left; color: red;width: 100%; border: 1px solid red; background-color: #FCDEDE\"><div><b>There were errors - data has not been saved!</b><br/></div></div>"));
//            foreach (Property property2 in this._content.GenericProperties)
//            {
//                if (hashtable[property2.PropertyType.Id.ToString()] == null)
//                {
//                    this.addControlNew(property2, this.PropertiesTab, ui.Text("general", "properties", null));
//                }
//            }
//        }

//        private void FireAfterContentControlLoad(ContentControlLoadEventArgs e)
//        {
//            if (_afterContenControlLoad != null)
//            {
//                _afterContenControlLoad(this, e);
//            }
//        }

//        private void FireBeforeContentControlLoad(ContentControlLoadEventArgs e)
//        {
//            if (_beforeContentControlLoad != null)
//            {
//                _beforeContentControlLoad(this, e);
//            }
//        }

//        protected override void OnInit(EventArgs e)
//        {
//            base.OnInit(e);
//            EnsureChildControls();
//            var args = new ContentControlLoadEventArgs();
//            FireBeforeContentControlLoad(args);
//            if (args.Cancel) 
//                return;

//            _txtName.ID = "NameTxt";
//            if (!Page.IsPostBack)
//            {
//                _txtName.Text = _content.Text;
//            }
//            _rfvName.ControlToValidate = _txtName.ID;
//            var variables = new[] { ui.Text("name") };
//            _rfvName.ErrorMessage = @" " + ui.Text("errorHandling", "errorMandatoryWithoutTab", variables, null) + "<br/>";
//            _rfvName.EnableClientScript = false;
//            _rfvName.Display = ValidatorDisplay.Dynamic;
//            _phName.Controls.Add(_txtName);
//            _phName.Controls.Add(_rfvName);
//            PropertiesPane.addProperty(ui.Text("general", "name", null), _phName);
//            var c = new Literal { Text = _content.User.Name };
//            PropertiesPane.addProperty(ui.Text("content", "createBy", null), c);
//            c = new Literal { Text = _content.CreateDateTime.ToString() };
//            PropertiesPane.addProperty(ui.Text("content", "createDate", null), c);
//            c = new Literal { Text = _content.Id.ToString() };
//            PropertiesPane.addProperty("Id", c);
//            PropertiesTab.Controls.AddAt(0, PropertiesPane);
//            PropertiesTab.Style.Add("text-align", "center");
//        }

//        protected override void OnLoad(EventArgs e)
//        {
//            base.OnLoad(e);
//            var args = new ContentControlLoadEventArgs();
//            this.FireAfterContentControlLoad(args);
//        }

//        private void OnSaveClicked(object sender, ImageClickEventArgs e)
//        {
//            var document = _content as Document;
//            if (document != null)
//            {
//                var args = new SaveEventArgs();
                
//                //document.FireBeforeSave(args);
//                if (args.Cancel)
//                {
//                    return;
//                }
//            }
//            foreach (IDataEditor editor in _dataFields)
//            {
//                editor.Save();
//            }
//            if (!string.IsNullOrEmpty(_txtName.Text))
//            {
//                _content.Text = _txtName.Text;
//            }
//            _onSave(this, new EventArgs());
//        }

//        private void OnSaveAndPublishClicked(object sender, ImageClickEventArgs e)
//        {
//            DoesPublish = true;
//            OnSaveClicked(sender, e);
//            _onSaveAndPublish(this, new EventArgs());
//        }

//        private void saveToPublish(object sender, ImageClickEventArgs e)
//        {
//            OnSaveClicked(sender, e);
//            _onSaveToPublish(this, new EventArgs());
//        }

//        private static void EmptyHandler(object sender, EventArgs e)
//        {
//        }

//        // Properties
//        public Content ContentObject
//        {
//            get
//            {
//                return _content;
//            }
//        }

//        public string ErrorMessage
//        {
//            set
//            {
//                _errorMessage = value;
//            }
//        }

//        // Nested Types
//        public delegate void AfterContentControlLoadEventHandler(CustomContentControl contentControl, ContentControlLoadEventArgs e);

//        public delegate void BeforeContentControlLoadEventHandler(CustomContentControl contentControl, ContentControlLoadEventArgs e);

//        public enum PublishModes
//        {
//            Publish,
//            SendToPublish,
//            NoPublish
//        }
//    }
//}
