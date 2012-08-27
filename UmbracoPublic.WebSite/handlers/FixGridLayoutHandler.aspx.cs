using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Ajax;
using LinqIt.Ajax.Parsing;
using LinqIt.Components;
using LinqIt.Components.Data;
using LinqIt.Utils.Extensions;
using LinqIt.Utils.Web;

namespace UmbracoPublic.WebSite.handlers
{
    public partial class FixGridLayoutHandler : System.Web.UI.Page
    {
        private static readonly string _removedMessage = "Module will be removed";
        private static readonly string _resizedMessage = "Module will be resized to {0} columns";
        private static readonly string _duplicateMessage = "The placeholder already contains the module. The module will be removed.";

        protected void Page_Load(object sender, EventArgs e)
        {
            litOutput.Text = HtmlWriter.Generate(w => RenderOutput(w));
            AjaxUtil.RegisterPageMethods(this);
        }

        private void RenderOutput(HtmlWriter writer)
        {
            var provider = new LinqIt.UmbracoCustomFieldTypes.UmbracoTreeModuleProvider(Request.QueryString["itemId"]);
            var placeholderData = provider.GetPlaceholderData();
            var layout = provider.GetLayout();
            var cells = layout.GetPlaceholderCells();
            foreach (var placeholder in placeholderData.Keys.Where(k => placeholderData[k].Items.Any()))
            {
                writer.AddAttribute("ref", placeholder);
                writer.RenderBeginTag(HtmlTextWriterTag.Div, "cell");
                writer.RenderFullTag(HtmlTextWriterTag.H3, placeholder);

                foreach (var item in placeholderData[placeholder].Items)
                {
                    var cell = cells.Where(c => string.Compare(c.Key, placeholder, true) == 0).FirstOrDefault();

                    writer.AddAttribute("ref", item.Id);
                    writer.AddClass("module");
                    if (item.IsLocal)
                        writer.AddClass("local");
                    else
                        writer.AddClass("global");

                    writer.RenderBeginTag(HtmlTextWriterTag.Div);
                    var gridItem = provider.GetItem(item.Id);
                    writer.RenderFullTag(HtmlTextWriterTag.H4, gridItem.Text);
                    
                    var columnSpan = item.ColumnSpan;
                    var options = GridModuleResolver.Instance.GetModuleColumnOptions(item.ModuleType);
                    if (cell != null && cell.ColumnSpan < columnSpan)
                    {
                        var validOptions = options.Where(o => o <= cell.ColumnSpan);
                        if (validOptions.Any())
                            columnSpan = validOptions.Max();
                        else
                            cell = null;
                    }
                    
                    RenderDropDown(writer, cells, cell, options);
                    writer.RenderBeginTag(HtmlTextWriterTag.Em);
                    //if (cell == null)
                    //{
                    //    writer.RenderFullTag(HtmlTextWriterTag.Span, _removedMessage, "alert");
                    //}
                    //else if (columnSpan < item.ColumnSpan)
                    //{
                    //    writer.RenderFullTag(HtmlTextWriterTag.Span, string.Format(_resizedMessage, columnSpan), "info");
                    //}
                    writer.RenderEndTag();
                    writer.RenderEndTag();
                }
                writer.RenderEndTag();
            }
        }

        private static void RenderDropDown(HtmlWriter writer, IEnumerable<GridCell> cells, GridCell selectedCell, IEnumerable<int> validOptions)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Select);
            writer.AddAttribute("value", "");
            if (selectedCell == null)
                writer.AddAttribute("selected","selected");
            writer.RenderFullTag(HtmlTextWriterTag.Option, "None");
            foreach (var cell in cells.Where(c => c.ColumnSpan >= validOptions.Min()))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Value, cell.Key);
                if (cell == selectedCell)
                    writer.AddAttribute("selected", "selected");
                writer.RenderFullTag(HtmlTextWriterTag.Option, cell.Key);
            }
            writer.RenderEndTag();
        }
    
        [AjaxMethod(AjaxType.Sync)]
        public static JSONObject HandleChange(JSONArray request)
        {
            var provider = new LinqIt.UmbracoCustomFieldTypes.UmbracoTreeModuleProvider(HttpContext.Current.Request.QueryString["itemId"]);
            var placeholderData = provider.GetPlaceholderData();

            var layout = provider.GetLayout().Rows.SelectMany(r => r.Cells).ToArray();
            
            var result = layout.ToDictionary(cell => cell.Key, cell => new GridPlaceholderData(cell.Key, cell.ColumnSpan));

            var response = new JSONObject();
            var messages = new JSONArray();
            response.AddValue("messages", messages);

            foreach (JSONObject replacement in request.Values)
            {
                var from = (string) replacement["from"];
                var id = (string) replacement["id"];
                var to = (string) replacement["to"];

                var message = new JSONObject();
                message.AddValue("ph", from);
                message.AddValue("id", id);

                if (string.IsNullOrEmpty(to))
                {
                    message.AddValue("type", "alert");
                    message.AddValue("text", _removedMessage);
                }
                else
                {
                    var oldItem = placeholderData[from].Items.Where(i => i.Id == id).FirstOrDefault();
                    var newItem = provider.GetItem(id);
                    newItem.ColumnSpan = oldItem.ColumnSpan;

                    var cell = result[to];
                    if (cell.Items.Where(i => i.Id == newItem.Id).Any())
                    {
                        message.AddValue("type", "alert");
                        message.AddValue("text", _duplicateMessage);
                    }
                    else
                    {
                        if (cell.Span < newItem.ColumnSpan)
                        {
                            newItem.ColumnSpan = GridModuleResolver.Instance.GetModuleColumnOptions(newItem.ModuleType).Where(o => o <= cell.Span).Max();
                            message.AddValue("type", "info");
                            message.AddValue("text", string.Format(_resizedMessage, newItem.ColumnSpan));
                        }
                        else
                        {
                            message.AddValue("type", "ok");
                        }
                        cell.AddItem(newItem);
                    }
                }
                messages.AddValue(message);
            }
            
            response.AddValue("hiddenId", HttpContext.Current.Request.QueryString["hiddenId"]);
            var values = new JSONArray();
            values.AddRange(result.Values.Select(v => v.ToJSON()).ToArray());
            response.AddValue("value", values.ToString());
            return response;
        }
    }
}