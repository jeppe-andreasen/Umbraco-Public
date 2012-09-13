using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Cms;
using LinqIt.Cms.Data;

namespace UmbracoPublic.WebSite.test
{
    public partial class QueryTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void OnQueryClicked(object sender, EventArgs e)
        {
            try
            {
                var entity = CmsService.Instance.SelectSingleItem<Entity>(txtQuery.Text);
                if (entity != null)
                    litOutput.Text = entity.Path;
                else
                    litOutput.Text = "Not found";
            }
            catch(Exception exc)
            {
                litOutput.Text = exc.ToString();
            }
        }

        protected void OnQueryMultipleClicked(object sender, EventArgs e)
        {
            try
            {
                var entities = CmsService.Instance.SelectItems<Entity>(txtQuery.Text);
                if (!entities.Any())
                    litOutput.Text = "No results";
                else
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (var entity in entities)
                    {
                        builder.AppendLine(entity.Path + "<br />");
                    }
                    litOutput.Text = builder.ToString();
                }
            }
            catch (Exception exc)
            {
                litOutput.Text = exc.ToString();
            }
        }
    }
}