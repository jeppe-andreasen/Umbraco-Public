using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Search;
using LinqIt.Utils.Web;

namespace UmbracoPublic.WebSite.test
{
    public partial class IndexViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (var service = new SearchService("site"))
            {
                litOutput.Text = HtmlWriter.Generate(w => GenerateOutput(w, service));
            }
        }

        private static void GenerateOutput(HtmlWriter writer, SearchService service)
        {
            writer.RenderBeginTag(HtmlTextWriterTag.Ul);
            foreach (var record in service.GetAllRecords())
            {
                var id = record.Id;
                writer.RenderBeginTag(HtmlTextWriterTag.Li);
                var href = "/test/IndexDetails.aspx?id=" + HttpUtility.UrlEncode(id);
                writer.RenderLinkTag(href, id);
                writer.RenderEndTag();
            }
            writer.RenderEndTag();
        }
    }
}