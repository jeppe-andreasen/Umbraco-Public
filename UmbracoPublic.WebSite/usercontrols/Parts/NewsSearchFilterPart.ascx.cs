using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using LinqIt.Utils.Extensions;
using UmbracoPublic.Logic.Services;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.WebSite.usercontrols.Parts
{
    public partial class NewsSearchFilterPart : BasePart
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterScriptInit("searchfilter");
            if (!IsPostBack)
            {
                txtQuery.Text = Request.QueryString["query"];
                
                ddlSubjects.AddDefaultItem();
                foreach (var subject in DataService.Instance.GetSubjects())
                    ddlSubjects.Items.Add(new ListItem(subject.Value, subject.Key.ToString()));

                if (!string.IsNullOrEmpty(Request.QueryString["subject"]))
                    ddlSubjects.SelectByText(Request.QueryString["subject"]);

                var page = CmsService.Instance.GetItem<Entity>();
                DateTime? from = null;
                DateTime? to = null;
                if (Regex.IsMatch(page.EntityName, @"^\d{4}$"))
                {
                    from = new DateTime(Convert.ToInt32(page.EntityName), 1,1);
                    to = from.Value.AddYears(1).AddDays(-1);
                }

                if (!string.IsNullOrEmpty(Request.QueryString["from"]))
                    txtFrom.Text = Request.QueryString["from"];
                else
                {
                    if (!from.HasValue)
                        from = new DateTime(DateTime.Today.Year, 1, 1);
                    txtFrom.Text = from.Value.ToString("dd-MM-yyyy");
                }
                    
                if (!string.IsNullOrEmpty(Request.QueryString["to"]))
                    txtTo.Text = Request.QueryString["to"];
                else
                {
                    if (!to.HasValue)
                        to = DateTime.Today;
                    txtTo.Text = to.Value.ToString("dd-MM-yyyy");
                }
            }
        }

        protected void OnSearchClicked(object sender, EventArgs e)
        {
            var url = HttpContext.Current.Request.Url.LocalPath + "?";
            var parameters = new NameValueCollection();
            if (!string.IsNullOrEmpty(txtQuery.Text))
                parameters.Add("query", txtQuery.Text);
            if (!string.IsNullOrEmpty(txtFrom.Text))
                parameters.Add("from", txtFrom.Text);
            if (!string.IsNullOrEmpty(txtTo.Text))
                parameters.Add("to", txtTo.Text);
            if (ddlSubjects.SelectedIndex > 0)
                parameters.Add("subject", ddlSubjects.SelectedItem.Text);
            url += parameters.ToUrlParameterList();
            Response.Redirect(url);
        }
    }
}