using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Cms;
using LinqIt.Search;
using LinqIt.Utils;
using LinqIt.Utils.Web;
using UmbracoPublic.Logic.Entities;
using UmbracoPublic.Logic.Modules;
using UmbracoPublic.Logic.Parts.Paging;
using UmbracoPublic.Logic.Services;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.Logic.Parts.Search
{
    public class NewsSearchResultPart : BasePart
    {
        private SearchResult _result;
        private Literal _output;
        private Pager _pager;
        private NewsListPage _page;
        private const int _defaultItemsPerPage = 10;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            _page = CmsService.Instance.GetItem<NewsListPage>();

            Controls.Add(new LiteralControl("<div class=\"news-list\">"));

            _output = new Literal();
            Controls.Add(_output);

            _pager = new Pager();
            _pager.ItemsPerPage = _defaultItemsPerPage;
            Controls.Add(_pager);

            Controls.Add(new LiteralControl("</div>"));
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            try
            {
                var filter = SearchFilter.FromUrl();
                if (!filter.From.HasValue && !filter.To.HasValue)
                {
                    DateTime? from;
                    DateTime? to;
                    Urls.GetNewsListDates(out from, out to);
                    filter.From = from;
                    filter.To = to;
                }

                filter.TemplateName = "NewsPage";
                filter.CategorizationIds =  _page.CategorizationIds;
                _result = DataService.Instance.PerformSearch(filter);
                _pager.Visible = _page.ShowPager;
                if (_pager.Visible)
                {
                    _pager.ItemsPerPage = _page.MaxItemsShown;
                    _pager.Initialize(_result.TotalResults);
                }
            }
            catch (Exception exc)
            {
                Logging.Log(LogType.Error, "Unable to load newslist part", exc);
                _output.Text = "Error loading NewsList, " + exc.Message;
            }

        }

        protected override void OnPreRender(EventArgs e)
        {
            try
            {
                _output.Text = HtmlWriter.Generate(w => RenderOutput(_page, w));
                base.OnPreRender(e);
            }
            catch (Exception exc)
            {
                Logging.Log(LogType.Error, "Unable to render newslist module", exc);
                _output.Text = "Error rendering NewsList, " + exc.Message;
            }
        }

        protected void RenderOutput(NewsListPage item, HtmlWriter writer)
        {
            IEnumerable<SearchRecord> records = _result.Records.OrderByDescending(r => r.GetDate("date"));
            records = _pager.Visible ? records.Skip(_pager.Skip).Take(_pager.Take) : records.Take(_page.MaxItemsShown);
            Snippets.RenderNewsResults(writer, records.ToArray());
        }
    }
}
