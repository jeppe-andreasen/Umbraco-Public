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
using UmbracoPublic.Logic.Entities;
using UmbracoPublic.Logic.Parts;
using UmbracoPublic.Logic.Services;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.WebSite.usercontrols.Parts
{
    public partial class NewsSearchFilterPart : BaseUCPart
    {
        private readonly List<DropDownList> _categorizationDropdowns = new List<DropDownList>();

        protected void Page_Load(object sender, EventArgs e)
        {
            ModuleScripts.RegisterInitScript("searchfilter");
            if (!IsPostBack)
            {
                txtQuery.Text = Request.QueryString["query"];

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

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            var categorizations = CategorizationFolder.Get();
            if (categorizations == null)
                return;
            var selectedCategorizations = new IdList(Request.QueryString["categorizations"]);
            foreach (var type in categorizations.Types)
            {
                if (!type.IsNewslistFilterOption)
                    continue;

                var items = type.Items.Where(i => !i.IsHiddenFromNewslistFilter).ToArray();
                if (!items.Any())
                    continue;

                var label = new Label();
                label.Text = type.EntityName;
                label.CssClass = "control-label";
                plhCategorizationFilters.Controls.Add(label);
                plhCategorizationFilters.Controls.Add(new LiteralControl("<div class=\"controls\">"));
                var ddl = new DropDownList();
                ddl.AddDefaultItem();
                foreach (var item in items)
                {
                    var listItem = new ListItem(item.EntityName, item.Id.ToString()); 
                    ddl.Items.Add(listItem);
                    if (selectedCategorizations.Contains(item.Id))
                        ddl.SelectedIndex = ddl.Items.IndexOf(listItem);
                }
                plhCategorizationFilters.Controls.Add(ddl);
                _categorizationDropdowns.Add(ddl);
                plhCategorizationFilters.Controls.Add(new LiteralControl("</div>"));
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
            string categorizations = _categorizationDropdowns.Select(d => d.SelectedValue).Where(v => !string.IsNullOrEmpty(v)).ToSeparatedString(",");
            if (!string.IsNullOrEmpty(categorizations))
                parameters.Add("categorizations", categorizations);
            url += parameters.ToUrlParameterList();
            Response.Redirect(url);
        }
    }
}