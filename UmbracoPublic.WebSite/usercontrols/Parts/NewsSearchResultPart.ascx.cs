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
using UmbracoPublic.Logic.Services;
using UmbracoPublic.WebSite.Utilities;

namespace UmbracoPublic.WebSite.usercontrols.Parts
{
    public partial class NewsSearchResultPart : BasePart
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
            litOutput.Text = HtmlWriter.Generate(w => Snippets.RenderNewsResults(w, _result, pager));
            base.OnPreRender(e);
        }

        //protected override void RenderPart(LinqIt.Utils.Web.HtmlWriter writer)
        //{
        //    

        //    Snippets.RenderNewsResults(writer, filter);
        //}
    }
}