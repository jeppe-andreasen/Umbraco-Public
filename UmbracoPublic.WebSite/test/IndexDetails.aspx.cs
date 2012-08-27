using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Search;

namespace UmbracoPublic.WebSite.test
{
    public partial class IndexDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (var service = new SearchService("site"))
            {
                string id = Request.QueryString["id"];
                var record = service.GetAllRecords().Where(r => r.Id == id).FirstOrDefault();


                var header = new TableRow();
                header.Cells.Add(new TableCell() {Text = "Field"});
                header.Cells.Add(new TableCell() {Text = "Value"});
                table.Rows.Add(header);

                var fields = service.GetAllTerms();
                foreach (var field in fields)
                {
                    var row = new TableRow();
                    row.Cells.Add(new TableCell() {Text = field});
                    row.Cells.Add(new TableCell() {Text = record.GetString(field)});
                    table.Rows.Add(row);
                }
            }
        }
    }
}