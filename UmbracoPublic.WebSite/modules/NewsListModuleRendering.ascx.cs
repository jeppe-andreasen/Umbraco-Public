using System;
using System.Web.UI;
using LinqIt.Search;
using LinqIt.Utils.Web;
using UmbracoPublic.Logic.Entities;
using UmbracoPublic.Logic.Services;
using UmbracoPublic.WebSite.Utilities;

namespace UmbracoPublic.WebSite.modules
{
    public partial class NewsListModuleRendering : BaseModule<NewsListModule>
    {
        private SearchResult _result;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            pager.ItemsPerPage = Module.ItemsPerPage;
        }

        public override int[] GetModuleColumnOptions()
        {
            return new[] { 3, 4, 6, 9, 12 };
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var filter = SearchFilter.FromUrl();
            filter.TemplateName = "NewsPage";
            filter.CategorizationIds = Module.CategorizationIds;
            _result = DataService.Instance.PerformSearch(filter);
            pager.Initialize(_result.TotalResults);
        }

        protected override void OnPreRender(EventArgs e)
        {
            litOutput.Text = HtmlWriter.Generate(w => RenderOutput(Module, w));
            pager.Visible = !Module.DisablePaging;
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

            Snippets.RenderNewsResults(writer, _result, pager);

            writer.RenderEndTag();
        }

        
    }
}