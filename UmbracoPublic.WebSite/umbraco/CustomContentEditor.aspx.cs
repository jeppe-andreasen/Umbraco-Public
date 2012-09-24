//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.HtmlControls;
//using System.Web.UI.WebControls;
//using ClientDependency.Core.Controls;
//using umbraco;
//using umbraco.BasePages;
//using umbraco.BusinessLogic;
//using umbraco.BusinessLogic.Actions;
//using umbraco.cms.businesslogic.template;
//using umbraco.cms.businesslogic.web;
//using umbraco.controls;
//using umbraco.IO;
//using umbraco.presentation;
//using umbraco.uicontrols;
//using umbraco.uicontrols.DatePicker;


//namespace UmbracoPublic.WebSite.umbraco
//{
//    public partial class CustomContentEditor : UmbracoEnsuredPage
//    {
//        // Fields
//        private CustomContentControl.PublishModes _canPublish;
//        private Document _document;
//        private bool _documentHasPublishedVersion;
//        private CustomContentControl _contentControl;
//        private DropDownList ddlDefaultTemplate = new DropDownList();
//        protected TextBox documentName;
//        private Literal domainText = new Literal();
//        protected HtmlInputHidden doPublish;
//        protected HtmlInputHidden doSave;
//        private LiteralControl dp = new LiteralControl();
//        private DateTimePicker dpExpire = new DateTimePicker();
//        private DateTimePicker dpRelease = new DateTimePicker();
//        protected Literal jsIds;
//        protected JsInclude JsInclude1;
//        protected JsInclude JsInclude2;
//        protected JsInclude JsInclude3;
//        private Literal l = new Literal();
//        private Pane linkProps = new Pane();
//        private Literal littPublishStatus = new Literal();
//        private int? m_ContentId = null;
//        protected PlaceHolder plc;
//        private Pane publishProps = new Pane();
//        protected TabView TabView1;
//        private Button UnPublish = new Button();

//        // Methods
//        private void addPreviewButton(ScrollingMenu menu, int id)
//        {
//            menu.InsertSplitter(2);
//            MenuIconI ni = menu.NewIcon(3);
//            ni.AltText = ui.Text("buttons", "showPage", base.getUser());
//            ni.OnClickCommand = "window.open('dialogs/preview.aspx?id=" + id + "','umbPreview')";
//            ni.ImageURL = SystemDirectories.Umbraco + "/images/editor/vis.gif";
//        }

//        private void AddTemplateDropDown(int defaultTemplate, ref PlaceHolder template, ref DocumentType DocumentType)
//        {
//            if (base.getUser().GetPermissions(this._document.Path).IndexOf(ActionUpdate.Instance.Letter) >= 0)
//            {
//                this.ddlDefaultTemplate.Items.Add(new ListItem(ui.Text("choose") + "...", ""));
//                foreach (Template template2 in DocumentType.allowedTemplates)
//                {
//                    ListItem item = new ListItem(template2.Text, template2.Id.ToString());
//                    if (template2.Id == defaultTemplate)
//                    {
//                        item.Selected = true;
//                    }
//                    this.ddlDefaultTemplate.Items.Add(item);
//                }
//                template.Controls.Add(this.ddlDefaultTemplate);
//            }
//            else if (defaultTemplate != 0)
//            {
//                template.Controls.Add(new LiteralControl(new Template(defaultTemplate).Text));
//            }
//            else
//            {
//                template.Controls.Add(new LiteralControl(ui.Text("content", "noDefaultTemplate")));
//            }
//        }

//        private bool CheckUserValidation()
//        {
//            if (!base.ValidateUserApp("content"))
//            {
//                this.ShowUserValidationError("The current user doesn't have access to this application. Please contact the system administrator.");
//                return false;
//            }
//            if (!base.ValidateUserNodeTreePermissions(this._document.Path, ActionBrowse.Instance.Letter.ToString()))
//            {
//                this.ShowUserValidationError("The current user doesn't have permissions to browse this document. Please contact the system administrator.");
//                return false;
//            }
//            if (!base.ValidateUserNodeTreePermissions(this._document.Path, ActionUpdate.Instance.Letter.ToString()))
//            {
//                this.ShowUserValidationError("The current user doesn't have permissions to edit this document. Please contact the system administrator.");
//                return false;
//            }
//            return true;
//        }

//        private static bool HideControls(Control c)
//        {
//            if (c is MasterPage)
//            {
//                return false;
//            }
//            if ((!(c is UserControl) && !(c is WebControl)) && !(c is HtmlForm))
//            {
//                return false;
//            }
//            c.Visible = false;
//            return true;
//        }

//        public static IEnumerable<Control> FlattenChildren(Control control)
//        {
//            var result = new List<Control>();
//            foreach (Control child in control.Controls)
//                AddControl(result, child);
//            return result;
//        }

//        private static void AddControl(List<Control> list, Control control)
//        {
//            list.Add(control);
//            foreach (Control child in control.Controls)
//                AddControl(list, child);
//        }


//        public static void DisplayFatalError(BasePage page, string msg)
//        {
//            foreach (Control control in page.Controls.Cast<Control>())
//            {
//                if (!HideControls(control))
//                {
//                    foreach (Control control2 in FlattenChildren(control))
//                    {
//                        HideControls(control2);
//                    }
//                    continue;
//                }
//            }
//            Feedback child = new Feedback
//            {
//                type = Feedback.feedbacktype.error,
//                Text = string.Format("<b>{0}</b><br/><br/>{1}", ui.GetText("error"), msg)
//            };
//            page.Controls.Add(child);
//        }



//        protected override void OnInit(EventArgs e)
//        {
//            int num;
//            base.OnInit(e);
//            if (!int.TryParse(base.Request.QueryString["id"], out num))
//            {
//                DisplayFatalError(this, "Invalid query string");
//            }
//            else
//            {
//                m_ContentId = new int?(num);
//                UnPublish.Click += new EventHandler(UnPublishDo);
//                _document = new Document(true, num);
//                if (string.IsNullOrEmpty(_document.Path))
//                {
//                    DisplayFatalError(this, "No document found with id " + m_ContentId);
//                    m_ContentId = null;
//                }
//                else
//                {
//                    _documentHasPublishedVersion = _document.HasPublishedVersion();
//                    if (!base.getUser().GetPermissions(_document.Path).Contains(ActionPublish.Instance.Letter.ToString()))
//                    {
//                        _canPublish = CustomContentControl.PublishModes.SendToPublish;
//                    }
//                    _contentControl = new CustomContentControl(_document, _canPublish, "TabView1");
//                    _contentControl.ID = "TabView1";
//                    _contentControl.Width = Unit.Pixel(0x29a);
//                    _contentControl.Height = Unit.Pixel(0x29a);
//                    foreach (TabPage page in _contentControl.GetPanels())
//                    {
//                        addPreviewButton(page.Menu, _document.Id);
//                    }
//                    plc.Controls.Add(_contentControl);
//                    PlaceHolder c = new PlaceHolder();
//                    if (_documentHasPublishedVersion)
//                    {
//                        littPublishStatus.Text = ui.Text("content", "lastPublished", base.getUser()) + ": " + _document.VersionDate.ToShortDateString() + " &nbsp; ";
//                        c.Controls.Add(littPublishStatus);
//                        if (base.getUser().GetPermissions(_document.Path).IndexOf("U") > -1)
//                        {
//                            UnPublish.Visible = true;
//                        }
//                        else
//                        {
//                            UnPublish.Visible = false;
//                        }
//                    }
//                    else
//                    {
//                        littPublishStatus.Text = ui.Text("content", "itemNotPublished", base.getUser());
//                        c.Controls.Add(littPublishStatus);
//                        UnPublish.Visible = false;
//                    }
//                    UnPublish.Text = ui.Text("content", "unPublish", base.getUser());
//                    UnPublish.ID = "UnPublishButton";
//                    UnPublish.Attributes.Add("onClick", "if (!confirm('" + ui.Text("defaultdialogs", "confirmSure", base.getUser()) + "')) return false; ");
//                    c.Controls.Add(UnPublish);
//                    publishProps.addProperty(ui.Text("content", "publishStatus", base.getUser()), c);
//                    var holder2 = new PlaceHolder();
//                    var type = new DocumentType(_document.ContentType.Id);
                    
//                    _contentControl.PropertiesPane.addProperty(ui.Text("documentType"), new LiteralControl(type.Text));
//                    _contentControl.PropertiesPane.addProperty(ui.Text("template"), holder2);
//                    var defaultTemplate = _document.Template != 0 ? _document.Template : type.DefaultTemplate;
//                    if (base.getUser().UserType.Name == "writer")
//                    {
//                        holder2.Controls.Add(defaultTemplate != 0
//                                                 ? new LiteralControl(Template.GetTemplate(defaultTemplate).Text)
//                                                 : new LiteralControl(ui.Text("content", "noDefaultTemplate")));
//                    }
//                    else
//                    {
//                        ddlDefaultTemplate.Items.Add(new ListItem(ui.Text("choose") + "...", ""));
//                        foreach (Template template in type.allowedTemplates)
//                        {
//                            var item = new ListItem(template.Text, template.Id.ToString());
//                            if (template.Id == defaultTemplate)
//                            {
//                                item.Selected = true;
//                            }
//                            ddlDefaultTemplate.Items.Add(item);
//                        }
//                        holder2.Controls.Add(ddlDefaultTemplate);
//                    }
//                    dp.ID = "updateDate";
//                    dp.Text = _document.UpdateDate.ToShortDateString() + " " + _document.UpdateDate.ToShortTimeString();
//                    publishProps.addProperty(ui.Text("content", "updateDate", base.getUser()), dp);
//                    dpRelease.ID = "releaseDate";
//                    dpRelease.DateTime = _document.ReleaseDate;
//                    dpRelease.ShowTime = true;
//                    publishProps.addProperty(ui.Text("content", "releaseDate", base.getUser()), dpRelease);
//                    dpExpire.ID = "expireDate";
//                    dpExpire.DateTime = _document.ExpireDate;
//                    dpExpire.ShowTime = true;
//                    publishProps.addProperty(ui.Text("content", "expireDate", base.getUser()), dpExpire);
//                    updateLinks();
//                    linkProps.addProperty(ui.Text("content", "urls", base.getUser()), l);
//                    if (domainText.Text != "")
//                    {
//                        linkProps.addProperty(ui.Text("content", "alternativeUrls", base.getUser()), domainText);
//                    }
//                    _contentControl.Save += Save;
//                    _contentControl.SaveAndPublish += Publish;
//                    _contentControl.SaveToPublish += SendToPublish;
//                    _contentControl.PropertiesTab.Controls.AddAt(1, publishProps);
//                    _contentControl.PropertiesTab.Controls.AddAt(2, linkProps);
//                    addPreviewButton(_contentControl.PropertiesTab.Menu, _document.Id);
//                }
//            }
//        }

//        protected void Page_Load(object sender, EventArgs e)
//        {
//            if (m_ContentId.HasValue && CheckUserValidation())
//            {
//                StateHelper.Cookies.Preview.Clear();
//                if (!base.IsPostBack)
//                {
//                    Log.Add(LogTypes.Open, base.getUser(), _document.Id, "");
//                    base.ClientTools.SyncTree(_document.Path, false);
//                }
//                jsIds.Text = "var umbPageId = " + _document.Id.ToString() + ";\nvar umbVersionId = '" + _document.Version.ToString() + "';\n";
//            }
//        }

//        protected void Publish(object sender, EventArgs e)
//        {
//            if (Page.IsValid)
//            {
//                if ((_document.Level == 1) || new Document(_document.Parent.Id).Published)
//                {
//                    base.Trace.Warn("before d.publish");
//                    if (_document.PublishWithResult(base.getUser()))
//                    {
//                        base.ClientTools.ShowSpeechBubble(BasePage.speechBubbleIcon.save, ui.Text("speechBubbles", "editContentPublishedHeader", null), ui.Text("speechBubbles", "editContentPublishedText", null));
//                        library.UpdateDocumentCache(_document.Id);
//                        Log.Add(LogTypes.Publish, base.getUser(), _document.Id, "");
//                        littPublishStatus.Text = ui.Text("content", "lastPublished", base.getUser()) + ": " + _document.VersionDate.ToString() + "<br/>";
//                        if (base.getUser().GetPermissions(_document.Path).IndexOf("U") > -1)
//                        {
//                            UnPublish.Visible = true;
//                        }
//                        _documentHasPublishedVersion = _document.HasPublishedVersion();
//                        updateLinks();
//                    }
//                    else
//                    {
//                        base.ClientTools.ShowSpeechBubble(BasePage.speechBubbleIcon.error, ui.Text("error"), ui.Text("contentPublishedFailedByEvent"));
//                    }
//                }
//                else
//                {
//                    base.ClientTools.ShowSpeechBubble(BasePage.speechBubbleIcon.error, ui.Text("error"), ui.Text("speechBubbles", "editContentPublishedFailedByParent"));
//                }
//            }
//        }

//        protected void Save(object sender, EventArgs e)
//        {
//            if (!Page.IsValid)
//            {
//                foreach (TabPage page in _contentControl.GetPanels())
//                {
//                    page.ErrorControl.Visible = true;
//                    page.ErrorHeader = ui.Text("errorHandling", "errorButDataWasSaved");
//                    page.CloseCaption = ui.Text("close");
//                }
//            }
//            else if (Page.IsPostBack)
//            {
//                foreach (TabPage page2 in _contentControl.GetPanels())
//                {
//                    page2.ErrorControl.Visible = false;
//                }
//            }
//            Log.Add(LogTypes.Save, base.getUser(), _document.Id, "");
//            if (_document.Text != _contentControl._txtName.Text)
//            {
//                _document.Text = _contentControl._txtName.Text;
//            }
//            if ((dpRelease.DateTime > new DateTime(0x6d9, 1, 1)) && (dpRelease.DateTime < new DateTime(0x270f, 12, 0x1f)))
//            {
//                _document.ReleaseDate = dpRelease.DateTime;
//            }
//            else
//            {
//                _document.ReleaseDate = new DateTime(1, 1, 1, 0, 0, 0);
//            }
//            if ((dpExpire.DateTime > new DateTime(0x6d9, 1, 1)) && (dpExpire.DateTime < new DateTime(0x270f, 12, 0x1f)))
//            {
//                _document.ExpireDate = dpExpire.DateTime;
//            }
//            else
//            {
//                _document.ExpireDate = new DateTime(1, 1, 1, 0, 0, 0);
//            }
//            if (ddlDefaultTemplate.SelectedIndex > 0)
//            {
//                _document.Template = int.Parse(ddlDefaultTemplate.SelectedValue);
//            }
//            else if (new DocumentType(_document.ContentType.Id).allowedTemplates.Length == 0)
//            {
//                _document.RemoveTemplate();
//            }
//            global::umbraco.BusinessLogic.Actions.Action.RunActionHandlers(_document, ActionUpdate.Instance);
//            _document.Save();
//            dp.Text = _document.UpdateDate.ToShortDateString() + " " + _document.UpdateDate.ToShortTimeString();
//            if (!_contentControl.DoesPublish)
//            {
//                base.ClientTools.ShowSpeechBubble(BasePage.speechBubbleIcon.save, ui.Text("speechBubbles", "editContentSavedHeader", null), ui.Text("speechBubbles", "editContentSavedText", null));
//            }
//            base.ClientTools.SyncTree(_document.Path, true);
//        }

//        protected void SendToPublish(object sender, EventArgs e)
//        {
//            if (Page.IsValid)
//            {
//                base.ClientTools.ShowSpeechBubble(BasePage.speechBubbleIcon.save, ui.Text("speechBubbles", "editContentSendToPublish", base.getUser()), ui.Text("speechBubbles", "editContentSendToPublishText", base.getUser()));
//                _document.SendToPublication(base.getUser());
//            }
//        }

//        private void ShowUserValidationError(string message)
//        {
//            Controls.Clear();
//            Controls.Add(new LiteralControl(string.Format("<h1>{0}</h1>", message)));
//        }

//        protected void UnPublishDo(object sender, EventArgs e)
//        {
//            _document.UnPublish();
//            littPublishStatus.Text = ui.Text("content", "itemNotPublished", base.getUser());
//            UnPublish.Visible = false;
//            library.UnPublishSingleNode(_document.Id);
//        }

//        private void updateLinks()
//        {
//            if (_documentHasPublishedVersion)
//            {
//                string str = library.NiceUrl(_document.Id);
//                l.Text = "<a href=\"" + str + "\" target=\"_blank\">" + str + "</a>";
//                domainText.Text = "";
//                foreach (string str2 in _document.Path.Split(new char[] { ',' }))
//                {
//                    if (int.Parse(str2) > -1)
//                    {
//                        Document document = new Document(int.Parse(str2));
//                        if (document.Published)
//                        {
//                            Domain[] domainsById = Domain.GetDomainsById(int.Parse(str2));
//                            if (domainsById.Length > 0)
//                            {
//                                for (int i = 0; i < domainsById.Length; i++)
//                                {
//                                    string str3 = "";
//                                    if (library.NiceUrl(int.Parse(str2)) == "")
//                                    {
//                                        str3 = "<em>N/A</em>";
//                                    }
//                                    else if (int.Parse(str2) != _document.Id)
//                                    {
//                                        string str4 = library.NiceUrl(int.Parse(str2));
//                                        string str5 = (str4 != "/") ? str.Replace(str4.Replace(".aspx", ""), "") : str;
//                                        if (!str5.StartsWith("/"))
//                                        {
//                                            str5 = "/" + str5;
//                                        }
//                                        str3 = "http://" + domainsById[i].Name + str5;
//                                    }
//                                    else
//                                    {
//                                        str3 = "http://" + domainsById[i].Name;
//                                    }
//                                    string text = domainText.Text;
//                                    domainText.Text = text + "<a href=\"" + str3 + "\" target=\"_blank\">" + str3 + "</a><br/>";
//                                }
//                            }
//                        }
//                        else
//                        {
//                            l.Text = "<i>" + ui.Text("content", "parentNotPublished", document.Text, base.getUser()) + "</i>";
//                        }
//                    }
//                }
//            }
//            else
//            {
//                l.Text = "<i>" + ui.Text("content", "itemNotPublished", base.getUser()) + "</i>";
//            }
//        }
//    }
//}

