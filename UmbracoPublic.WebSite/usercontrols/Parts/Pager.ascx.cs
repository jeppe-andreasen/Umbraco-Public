using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Utils.Extensions;
using LinqIt.Utils.Web;
using UmbracoPublic.Logic.Controllers.Paging;
using UmbracoPublic.Logic.Parts;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.WebSite.usercontrols.Parts
{
    public enum PagerType { Default, Centered, PagerCentered, PagerAligned }

    public partial class Pager : BaseUCPart
    {
        


        protected void Page_Load(object sender, EventArgs e)
        {
        }

        //private void GenerateLinks(HtmlWriter writer)
        //{
        //    var paging = new PagerHelper(_totalCount, MaxPagesShown, ItemsPerPage, PageNumber);
        //    Visible = paging.Pages > 0;
        //    if (!Visible)
        //        return;

        //    writer.RenderBeginDiv(null, "pages");
        //    writer.RenderBeginTag(HtmlTextWriterTag.Ul);

        //    if (PageNumber > 1)
        //        RenderPageLink(writer, PageNumber - 1, "Forrige side", "back", true);

        //    if (ShowEnds && paging.FirstPage > 1)
        //    {
        //        string text = paging.FirstPage > 2 ? "1.." : "1";
        //        RenderPageLink(writer, 1, text, null, false);
        //    }

        //    for (int i = paging.FirstPage; i <= paging.LastPage; i++)
        //        RenderPageLink(writer, i == PageNumber ? (int?)null : i, i.ToString(), null, false);

        //    if (ShowEnds && paging.LastPage < paging.Pages)
        //    {
        //        string text = (paging.Pages - paging.LastPage > 1) ? ".." + paging.Pages : paging.Pages.ToString();
        //        RenderPageLink(writer, paging.Pages, text, null, false);
        //    }

        //    if (PageNumber < paging.Pages)
        //        RenderPageLink(writer, PageNumber + 1, "Næste side", "next", true);

        //    writer.RenderEndTag();
        //    writer.RenderEndTag();
        //}

        //private static void RenderPageLink(HtmlWriter writer, int? pageLink, string text, string cssClass, bool addSpan)
        //{
        //    writer.AddClass(cssClass);
        //    writer.RenderBeginTag(HtmlTextWriterTag.Li);
        //    if (pageLink.HasValue)
        //    {
        //        writer.AddAttribute(HtmlTextWriterAttribute.Title, text);
        //        writer.RenderBeginLink(Urls.ReplaceUrlParameter(QueryStringKey.PageNumber, pageLink.Value.ToString()));
        //    }
        //    else
        //        writer.RenderBeginTag(HtmlTextWriterTag.Strong);

        //    if (addSpan)
        //        writer.RenderBeginTag(HtmlTextWriterTag.Span);
        //    writer.Write(text);
        //    if (addSpan)
        //        writer.RenderEndTag();

        //    writer.RenderEndTag(); // a or strong
        //    writer.RenderEndTag(); // li
        //}

        

        //protected void OnRangeTranslated(object sender, TranslationEventArgs e)
        //{
        //    var count = _totalCount;
        //    var firstItem = Skip + 1;
        //    var lastItem = Math.Min(firstItem + ItemsPerPage - 1, count);
        //    e.Embed(firstItem, lastItem, count);
        //}

        //public void GenerateItemsPerPage(HtmlWriter writer)
        //{
        //    if (string.IsNullOrEmpty(ItemsPerPageOptions))
        //        return;

        //    writer.RenderBeginDiv(null, "items-per-page");
        //    writer.RenderFullTag(HtmlTextWriterTag.Span, "Links pr. side");
        //    var options = ItemsPerPageOptions.Split(',', '|').Select(i => Convert.ToInt32(i)).ToArray();

        //    var urlReplacements = new Dictionary<QueryStringKey, string> { { QueryStringKey.PageNumber, string.Empty } };

        //    writer.RenderBeginTag(HtmlTextWriterTag.Ul);
        //    foreach (var option in options)
        //    {
        //        writer.RenderBeginTag(HtmlTextWriterTag.Li);
        //        if (option == ItemsPerPage)
        //            writer.RenderFullTag(HtmlTextWriterTag.Strong, option.ToString());
        //        else
        //        {
        //            urlReplacements[QueryStringKey.ItemsPerPage] = option.ToString();
        //            var url = Urls.ReplaceUrlParameters(urlReplacements);
        //            writer.RenderLinkTag(url, option.ToString());
        //        }
        //        writer.RenderEndTag();
        //    }
        //    writer.RenderEndTag();
        //    writer.RenderEndTag();
        //}

        
    
        //private void RenderDefault(HtmlWriter writer)
        //{
        //    var paging = new PagerHelper(_totalCount, MaxPagesShown, ItemsPerPage, PageNumber);
        //    Visible = paging.Pages > 0;
        //    if (!Visible)
        //        return;

        //    writer.RenderBeginTag(HtmlTextWriterTag.Div, "pagination");
        //    writer.RenderBeginTag(HtmlTextWriterTag.Ul);
        //    writer.RenderBeginTag(HtmlTextWriterTag.Li);
        //    writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
        //    writer.RenderFullTag(HtmlTextWriterTag.A, "Prev");
        //    writer.RenderEndTag(); // li
        //    writer.RenderBeginTag(HtmlTextWriterTag.Li, "active");
        //    writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
        //    writer.RenderFullTag(HtmlTextWriterTag.A, "1");
        //    writer.RenderEndTag(); // li.active
        //    writer.RenderBeginTag(HtmlTextWriterTag.Li);
        //    writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
        //    writer.RenderFullTag(HtmlTextWriterTag.A, "2");
        //    writer.RenderEndTag(); // li
        //    writer.RenderBeginTag(HtmlTextWriterTag.Li);
        //    writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
        //    writer.RenderFullTag(HtmlTextWriterTag.A, "3");
        //    writer.RenderEndTag(); // li
        //    writer.RenderBeginTag(HtmlTextWriterTag.Li);
        //    writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
        //    writer.RenderFullTag(HtmlTextWriterTag.A, "4");
        //    writer.RenderEndTag(); // li
        //    writer.RenderBeginTag(HtmlTextWriterTag.Li);
        //    writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
        //    writer.RenderFullTag(HtmlTextWriterTag.A, "Next");
        //    writer.RenderEndTag(); // li
        //    writer.RenderEndTag(); // ul
        //    writer.RenderEndTag(); // div.pagination

        //}
    
    
    }
}