using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Cms;
using LinqIt.Components;
using LinqIt.Components.Data;
using LinqIt.Utils.Extensions;
using LinqIt.Utils.Web;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.web;

namespace UmbracoPublic.WebSite.handlers.Dialogs
{
    public partial class CreateModuleDialog : DialogPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public override DialogResponse HandleOk()
        {
            var provider = ProviderHelper.GetGridItemProvider(Request.QueryString["provider"], null);
            GridItem item = provider.CreateItem(txtName.Text, Request.QueryString["pid"], Request.QueryString["tid"]);
            var response = new DialogResponse("CreateModule", true);
            response.AddValue("x", (int)Convert.ToDecimal(Request.QueryString["x"], CultureInfo.InvariantCulture));
            response.AddValue("y", (int)Convert.ToDecimal(Request.QueryString["y"], CultureInfo.InvariantCulture));
            response.AddValue("ph", Request.QueryString["ph"]);
            response.AddValue("id", item.Id);
            
            response.AddValue("html", HtmlWriter.Generate(writer => LinqItGridEditor.RenderModule(writer, provider, item)));

            response.AddCommand(DialogCommand.ShowDialog, "CustomContentEditor," + item.Id);

            return response;
        }
    }
}

                //writer.AddAttribute("ref", item.Id);
                //writer.AddAttribute(HtmlTextWriterAttribute.Style, "position: absolute;");
                //writer.RenderBeginTag(HtmlTextWriterTag.Div, "module draggable");
                //writer.RenderBeginTag(HtmlTextWriterTag.H3);
                //writer.AddAttribute(HtmlTextWriterAttribute.Alt, "");
                //writer.AddAttribute(HtmlTextWriterAttribute.Src, item.Icon);
                //writer.RenderFullTag(HtmlTextWriterTag.Img, "");
                //writer.RenderFullTag(HtmlTextWriterTag.Span, item.Text);

                //writer.AddAttribute(HtmlTextWriterAttribute.Title, "edit");
                //writer.AddClass("edit");
                //writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                //writer.RenderFullTag(HtmlTextWriterTag.A, "&nbsp;");

                //writer.AddAttribute(HtmlTextWriterAttribute.Title, "remove");
                //writer.AddClass("remove");
                //writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
                //writer.RenderFullTag(HtmlTextWriterTag.A, "&nbsp;");

                //writer.RenderEndTag(); // h3
                //writer.RenderBeginTag(HtmlTextWriterTag.P);
                //writer.RenderFullTag(HtmlTextWriterTag.Span, item.ColumnSpan.ToString(), "cols");
                //writer.RenderEndTag(); // p
                //writer.RenderEndTag(); // div.module draggable ui-draggable selected