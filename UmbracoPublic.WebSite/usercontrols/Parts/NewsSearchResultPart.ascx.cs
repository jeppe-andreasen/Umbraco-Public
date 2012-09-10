using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Cms.Data;
using LinqIt.Search;
using LinqIt.Utils.Web;
using UmbracoPublic.Logic.Entities;
using UmbracoPublic.Logic.Modules;
using UmbracoPublic.Logic.Parts;
using UmbracoPublic.Logic.Services;
using UmbracoPublic.WebSite.Utilities;

namespace UmbracoPublic.WebSite.usercontrols.Parts
{
    public partial class NewsSearchResultPart : BaseUCPart
    {
        private SearchResult _result;

        protected void Page_Load(object sender, EventArgs e)
        {
            var filter = SearchFilter.FromUrl();
            filter.TemplateName = "NewsPage";

            _result = DataService.Instance.PerformSearch(filter);
            pager.Initialize(_result.TotalResults);
        }

        protected override void OnPreRender(EventArgs e)
        {
            IEnumerable<SearchRecord> records = _result.Records.OrderByDescending(r => r.GetDate("date"));
            if (pager.Visible)
                records = records.Skip(pager.Skip).Take(pager.Take);
            litOutput.Text = HtmlWriter.Generate(w => GenerateOutput(w, records));

            base.OnPreRender(e);
        }

        private static void GenerateOutput(HtmlWriter writer, IEnumerable<SearchRecord> records)
        {

            Snippets.RenderNewsResults(writer, records.ToArray());
        }
    }
}