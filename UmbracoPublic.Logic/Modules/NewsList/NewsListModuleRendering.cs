using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Search;
using LinqIt.Utils.Web;
using UmbracoPublic.Logic.Entities;
using UmbracoPublic.Logic.Parts.Paging;
using UmbracoPublic.Logic.Services;

namespace UmbracoPublic.Logic.Modules.NewsList
{
    public class NewsListModuleRendering : BaseModuleRendering<NewsListModule>
    {
        private SearchResult _result;
        private Literal _output;
        private Pager _pager;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            _output = new Literal();
            Controls.Add(_output);

            _pager = new Pager();
            _pager.ItemsPerPage = Module.ItemsPerPage;
            Controls.Add(_pager);
            
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var filter = SearchFilter.FromUrl();
            filter.TemplateName = "NewsPage";
            filter.CategorizationIds = Module.CategorizationIds;
            _result = DataService.Instance.PerformSearch(filter);
            _pager.Visible = Module.ShowPager;
            if (_pager.Visible)
            {
                if (Module.MaxItemsShown.HasValue)
                    _pager.ItemsPerPage = Module.MaxItemsShown.Value;
                _pager.Initialize(_result.TotalResults);
            }

        }

        protected override void OnPreRender(EventArgs e)
        {
            _output.Text = HtmlWriter.Generate(w => RenderOutput(Module, w));
            base.OnPreRender(e);
        }

        protected void RenderOutput(NewsListModule item, HtmlWriter writer)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Div, "news-list");

            var renderHr = false;
            if (!string.IsNullOrEmpty(item.Headline))
            {
                writer.RenderFullTag(HtmlTextWriterTag.H1, item.Headline);
                writer.WriteBreak();
                renderHr = true;
            }
            if (!item.Intro.IsEmpty)
            {
                writer.RenderParagraph(item.Intro.AsHtml);
                renderHr = true;
            }
            if (renderHr)
                writer.RenderFullTag(HtmlTextWriterTag.Hr, "");

            IEnumerable<SearchRecord> records = _result.Records.OrderByDescending(r => r.GetDate("date"));
            if (_pager.Visible)
                records = records.Skip(_pager.Skip).Take(_pager.Take);
            else if (Module.MaxItemsShown.HasValue)
                records = records.Take(Module.MaxItemsShown.Value);
            Snippets.RenderNewsResults(writer, records.ToArray());

            writer.RenderEndTag();
        }
    }
}
