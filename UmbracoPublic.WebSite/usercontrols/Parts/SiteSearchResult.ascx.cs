using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Search;

namespace UmbracoPublic.WebSite.usercontrols.Parts
{
    public partial class SiteSearchResult : BasePart
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void RenderPart(LinqIt.Utils.Web.HtmlWriter writer)
        {
            using (var service = new SearchService("site"))
            {
                var q = BooleanQuery.Or(Request.QueryString["query"].ToLower().Split(' ').Where(s => !string.IsNullOrEmpty(s)).Select(s => new WildCardQuery("text", "*" + s + "*")).ToArray());
                var result = service.Search(q, 0, int.MaxValue);
                foreach (var record in result.Records)
                {
                    writer.RenderBeginTag(HtmlTextWriterTag.Div, "search-result");
                    writer.RenderBeginTag(HtmlTextWriterTag.H3);
                    writer.RenderLinkTag(record.GetString("url"), record.GetString("title"));
                    writer.RenderEndTag();
                    writer.RenderFullTag(HtmlTextWriterTag.Div, record.GetString("text"));
                    writer.RenderEndTag();
                }
            }
        }
    }
}