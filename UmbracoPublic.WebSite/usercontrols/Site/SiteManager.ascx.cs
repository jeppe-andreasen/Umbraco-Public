using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UmbracoPublic.Interfaces.SiteManagement;
using UmbracoPublic.Logic.Services;
using umbraco.cms.businesslogic.web;
using umbraco.editorControls.userControlGrapper;

namespace UmbracoPublic.WebSite.usercontrols.Site
{
    public partial class SiteManager : System.Web.UI.UserControl, IUsercontrolDataEditor
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "siteState", "$(function() { $('#siteState').accordion(); });", true);
        }

        protected override void CreateChildControls()
        {
            var siteRootId = Convert.ToInt32(Request.QueryString["id"]);
            var siteRoot = new Document(siteRootId);

            Controls.Add(new LiteralControl("<div id=\"siteState\">"));

            var siteComponents = DataService.GetSiteComponents();
            foreach (var component in siteComponents)
            {
                component.Initialize(siteRoot);

                Controls.Add(new LiteralControl("<h3 class=\"" + component.State.ToString() + "\"><a href=\"#\">" + component.Name + "</a></h3>"));
                Controls.Add(new LiteralControl("<div>"));
                Controls.Add(new LiteralControl("<div class=\"component-output\">"));
                component.InstantiateIn(Controls);
                Controls.Add(new LiteralControl("</div>"));
                Controls.Add(new LiteralControl("</div>"));
            }
            Controls.Add(new LiteralControl("</div>"));
        }

        public object value
        {
            get
            {
                return string.Empty;
            }
            set
            {
            }
        }
    }
}