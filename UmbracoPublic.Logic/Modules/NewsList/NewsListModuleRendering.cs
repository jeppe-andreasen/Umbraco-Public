using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Search;
using LinqIt.Utils;
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

            Controls.Add(new LiteralControl("<div class=\"news-list\">"));

            _output = new Literal();
            Controls.Add(_output);

            _pager = new Pager();
            _pager.ItemsPerPage = Module.ItemsPerPage;
            Controls.Add(_pager);
            
            Controls.Add(new LiteralControl("</div>"));
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            try
            {
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
            catch (Exception exc)
            {
                Logging.Log(LogType.Error, "Unable to load newslist module", exc);
                _output.Text = "Error loading NewsList, " + exc.Message;
            }

        }

        protected override void OnPreRender(EventArgs e)
        {
            try
            {
                _output.Text = HtmlWriter.Generate(w => RenderOutput(Module, w));
                base.OnPreRender(e);
            }
            catch(Exception exc)
            {
                Logging.Log(LogType.Error, "Unable to render newslist module", exc);
                _output.Text = "Error rendering NewsList, " + exc.Message;
            }
        }

        protected void RenderOutput(NewsListModule item, HtmlWriter writer)
        {
            if (!string.IsNullOrEmpty(item.Headline))
            {
                writer.RenderFullTag(HtmlTextWriterTag.H2, item.Headline);
            }
            if (!item.Intro.IsEmpty)
            {
                writer.RenderParagraph(item.Intro.AsHtml);
            }

            IEnumerable<SearchRecord> records = _result.Records.OrderByDescending(r => r.GetDate("date"));
            if (_pager.Visible)
                records = records.Skip(_pager.Skip).Take(_pager.Take);
            else if (Module.MaxItemsShown.HasValue)
                records = records.Take(Module.MaxItemsShown.Value);
            Snippets.RenderNewsResults(writer, records.ToArray());
        }

        public override string ModuleDescription
        {
            get { return "Nyhedsliste"; }
        }
    }
}
