//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Web;
//using LinqIt.Cms;
//using LinqIt.Cms.Data;
//using LinqIt.Parsing.Css;
//using LinqIt.Parsing.Html;
//using umbraco.BusinessLogic;
//using umbraco.cms.businesslogic;
//using umbraco.cms.businesslogic.web;
//using Page = System.Web.UI.Page;

//namespace UmbracoPublic.Logic.Events
//{
//    public class RichTextProcessor : global::umbraco.BusinessLogic.ApplicationBase
//    {
//        public RichTextProcessor()
//        {

//            Document.AfterSave += Document_AfterSave;
//        }

//        public static void Document_AfterSave(Document sender, SaveEventArgs e)
//        {
//            using (CmsContext.Editing)
//            {
                
//                CmsService.Instance.GetItem<Entity>();
//            }
            

//            var documentType = DocumentType.GetByAlias(sender.ContentType.Alias);
//            bool madeChanges = false;
//            foreach (var propertyType in documentType.PropertyTypes)
//            {
//                if (propertyType.DataTypeDefinition.Text == "Richtext editor")
//                {
//                    var property = sender.getProperty(propertyType);
//                    if (property == null)
//                        continue;

//                    string html = property.Value.ToString();
//                    if (string.IsNullOrEmpty(html))
//                        continue;

//                    var doc = new HtmlDocument(html);

//                    var cssQuery = CssSelectorStack.Parse(".alert");
//                    var tags = doc.FindElements(element => (element is HtmlTag) && element.Matches(cssQuery)).Cast<HtmlTag>();
//                    foreach (var t in tags.Where(t => !t.IsType("div")))
//                    {
//                        t.ChangeType("div");
//                        var text = t.InnerText;
//                        t.InnerHtml = string.Format("<button class=\"close\" data-dismiss=\"alert\">×</button><span>{0}</span>", text);
//                        madeChanges = true;
//                    }
//                    if (madeChanges)
//                        property.Value = doc.ToString();
//                }
//            }
//            if (madeChanges)
//            {
//                //sender.Save();
//                var handler = HttpContext.Current.Handler;
//                if (handler is Page)
//                {
//                    var page = (Page)handler;
//                    if (!page.ClientScript.IsClientScriptBlockRegistered(page.GetType(), "richTextReload"))
//                        page.ClientScript.RegisterStartupScript(page.GetType(), "richTextReload", "window.parent.openContent('" + sender.Id + "');", true);
//                }
//            }
//        }
//    }
//}
