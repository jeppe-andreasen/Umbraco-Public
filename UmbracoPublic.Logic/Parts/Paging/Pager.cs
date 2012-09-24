using System;
using System.Linq;
using System.Web;
using LinqIt.Utils.Extensions;
using LinqIt.Utils.Web;
using UmbracoPublic.Logic.Controllers.Paging;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.Logic.Parts.Paging
{
    public class Pager : BasePart
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
            PageNumber = HttpContext.Current.Request.GetQueryStringValue(QueryStringKey.PageNumber, 1);
            if (ItemsPerPage == 0)
            {
                var defaultItemsPerPage = 10;
                if (!string.IsNullOrEmpty(ItemsPerPageOptions))
                    defaultItemsPerPage = ItemsPerPageOptions.Split(',', '|').Select(i => Convert.ToInt32(i)).FirstOrDefault();
                ItemsPerPage = HttpContext.Current.Request.GetQueryStringValue(QueryStringKey.ItemsPerPage, defaultItemsPerPage);
            }

            if (MaxPagesShown == 0)
                MaxPagesShown = 10;
        }

        protected override void RenderPart(HtmlWriter writer)
        {
            GenerateOutput(writer);
        }

        public void GenerateOutput(HtmlWriter writer, bool renderOuterTag = true)
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
                default:
                    controller = new DefaultPagingController();
                    break;
            }

            controller.Render(writer, PageNumber, helper.FirstPage, helper.LastPage, helper.Pages, ShowEnds, renderOuterTag);
        }

        public int PageNumber { get; set; }

        public int ItemsPerPage { get; set; }

        public int MaxPagesShown { get; set; }

        public string ItemsPerPageOptions { get; set; }

        public bool ShowEnds { get; set; }

        public int Skip { get { return (PageNumber - 1) * ItemsPerPage; } }

        public int Take { get { return ItemsPerPage; } }

        public void Initialize(long totalCount)
        {
            _totalCount = Convert.ToInt32(totalCount);
        }
    }
}
