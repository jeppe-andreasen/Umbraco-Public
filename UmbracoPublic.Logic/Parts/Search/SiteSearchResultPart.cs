using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Cms;
using LinqIt.Cms.Data;
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
    public class SiteSearchResultPart : BasePart
    {
        private SearchResult _result;
        private Literal _output;
        private Pager _pager;
        private SiteSearchResultPage _page;
        private const int _defaultItemsPerPage = 10;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            _page = CmsService.Instance.GetItem<SiteSearchResultPage>();

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

                _result = DataService.Instance.PerformSearch(filter);
                _pager.Visible = true;
                if (_pager.Visible)
                {
                    _pager.ItemsPerPage = _page.MaxItemsShown;
                    _pager.Initialize(_result.TotalResults);
                }
            }
            catch (Exception exc)
            {
                Logging.Log(LogType.Error, "Unable to load sitesearchresult part", exc);
                _output.Text = "Error loading SearchResult, " + exc.Message;
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
                Logging.Log(LogType.Error, "Unable to render sitesearchresult module", exc);
                _output.Text = "Error rendering SearchResult, " + exc.Message;
            }
        }

        protected void RenderOutput(SiteSearchResultPage item, HtmlWriter writer)
        {
            IEnumerable<SearchRecord> records = _result.Records;
            records = _pager.Visible ? records.Skip(_pager.Skip).Take(_pager.Take) : records.Take(_page.MaxItemsShown);
            var page = CmsService.Instance.GetItem<SiteSearchResultPage>();
            if (!page.Intro.IsEmpty)
            {
                var query = SearchFilter.FromUrl().Query;
                var intro = page.Intro.AsHtml.Replace("[QUERY]",
                                                      string.IsNullOrEmpty(query)
                                                          ? ""
                                                          : string.Format("<span class=\"search-word\">{0}</span>",
                                                                          HttpUtility.HtmlEncode(query)));
                writer.RenderFullTag(HtmlTextWriterTag.H2, intro, "intro");
            }

            RenderResults(writer, records.ToArray());
        }

        public static void RenderResults(HtmlWriter writer, SearchRecord[] records, bool renderUl = true)
        {
            var visibleCategorizations = CategorizationFolder.GetVisibleCategorizations();
            var newsListUrl = Urls.GetMainNewsListUrl();

            if (renderUl)
                writer.RenderBeginTag(HtmlTextWriterTag.Ul, "news-list");
            foreach (var record in records)
            {
                var date = record.GetDate("date");

                writer.RenderBeginTag(HtmlTextWriterTag.Li, "clearfix");

                writer.RenderBeginTag(HtmlTextWriterTag.Div);
                writer.AddAttribute(HtmlTextWriterAttribute.Href, record.GetString("url"));
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.RenderFullTag(HtmlTextWriterTag.H3, record.GetString("title"));
                writer.RenderEndTag(); // a
                if (date != null)
                    writer.RenderFullTag(HtmlTextWriterTag.Span,
                                         "Publiseret " + record.GetDate("date").Value.ToString("dd-MM-yyyy"), "date");

                var categorizations = new IdList(record.GetString("categorizations"));
                if (categorizations.Any())
                {
                    RenderCategorizations(writer, categorizations, visibleCategorizations, newsListUrl);
                }
                writer.RenderBeginTag(HtmlTextWriterTag.A);

                var text = record.GetString("summary");
                if (text.Length > 150)
                    text = text.Substring(0, 150);
                writer.RenderFullTag(HtmlTextWriterTag.P, text);

                writer.RenderEndTag(); // a
                writer.RenderEndTag(); // div
                writer.RenderEndTag(); // li.clearfix
            }
            if (renderUl)
                writer.RenderEndTag();
        }

        public static void RenderCategorizations(HtmlWriter writer, IEnumerable<Id> categorizations, Dictionary<Id, Categorization> visibleCategorizations, string newsListUrl = null)
        {
            if (newsListUrl == null)
                newsListUrl = Urls.GetMainNewsListUrl();

            foreach (var categorizationId in categorizations)
            {
                if (!visibleCategorizations.ContainsKey(categorizationId))
                    continue;

                writer.RenderLinkTag(newsListUrl + "?categorizations=" + categorizationId, visibleCategorizations[categorizationId].DisplayName, "label");
            }
        } 
    }
}