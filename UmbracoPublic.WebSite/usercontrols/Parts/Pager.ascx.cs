using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Utils.Extensions;
using LinqIt.Utils.Web;
using UmbracoPublic.Logic.Controllers.Paging;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.WebSite.usercontrols.Parts
{
    public enum PagerType { Default, Centered, PagerCentered, PagerAligned }

    public partial class Pager : BasePart
    {
        private int _totalCount;

        public void SetPageNumber(int pageNumber)
        {
            PageNumber = pageNumber;
        }

        public PagerType Type
        {
            get { return (PagerType)(ViewState["Type"] ?? PagerType.Default); }
            set { ViewState["Type"] = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PageNumber = Request.GetQueryStringValue(QueryStringKey.PageNumber, 1);
            if (ItemsPerPage == 0)
            {
                var defaultItemsPerPage = 10;
                if (!string.IsNullOrEmpty(ItemsPerPageOptions))
                    defaultItemsPerPage = ItemsPerPageOptions.Split(',', '|').Select(i => Convert.ToInt32(i)).FirstOrDefault();
                ItemsPerPage = Request.GetQueryStringValue(QueryStringKey.ItemsPerPage, defaultItemsPerPage);
            }

            if (MaxPagesShown == 0)
                MaxPagesShown = 10;
        }

        protected override void RenderPart(HtmlWriter writer)
        {
            var helper = new PagerHelper(_totalCount, MaxPagesShown, ItemsPerPage, PageNumber);
            if (helper.Pages <= 1)
                return;

            PagingController controller;
            switch (Type)
            {
                case PagerType.Default:
                    controller = new DefaultPagingController();
                    break;
                case PagerType.Centered:
                    controller = new CenteredPagingController();
                    break;
                case PagerType.PagerCentered:
                    controller = new PagerCenteredPagingController();
                    break;
                    controller = new PagerAlignedPagingController();
                    break;
                default:
                    controller = new DefaultPagingController();
                    break;
            }
            
            controller.Render(writer, PageNumber, helper.FirstPage, helper.LastPage, helper.Pages, ShowEnds);
        }


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

        public int PageNumber { get; private set; }

        public int ItemsPerPage { get; set; }

        public int MaxPagesShown { get; set; }

        public string ItemsPerPageOptions { get; set; }

        public bool ShowEnds { get; set; }

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

        public int Skip { get { return (PageNumber - 1) * ItemsPerPage; } }

        public int Take { get { return ItemsPerPage; } }

        internal void Initialize(long totalCount)
        {
            _totalCount = Convert.ToInt32(totalCount);
        }
    
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