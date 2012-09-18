using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using UmbracoPublic.Logic.Entities;
using UmbracoPublic.Logic.Services;

namespace UmbracoPublic.Logic.Macros
{
    public class CookieAcceptanceMacro : Control
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateMode = ViewStateMode.Enabled;
            EnsureChildControls();
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();

            var cookiesAccepted = DataService.Instance.GetCookieState() == CookieState.Accepted;
            AddLiteral(!cookiesAccepted ? "<div class=\"alert alert-block\">" : "<div class=\"alert alert-block alert-info\">");
            AddLiteral("<button type=\"button\" class=\"close\" data-dismiss=\"alert\">×</button>");
            AddLiteral("<h4>Accept af cookies fra " + this.Page.Request.Url.Host  + "</h4>");
            if (cookiesAccepted)
                AddLiteral("Vi har gemt cookies på din computer, da du tidligere har accepteret dem på " + Page.Request.Url.Host);

            var btnSubmit = new Button();
            btnSubmit.ID = "btnSubmit";
            

            if (cookiesAccepted)
            {
                btnSubmit.Click += OnDeleteClicked;
                btnSubmit.Text += "Slet cookies.";
                btnSubmit.CssClass = "btn btn-danger";
            }
            else
            {
                btnSubmit.Click += OnAcceptClicked;
                btnSubmit.Text = "Accepter cookies.";
                btnSubmit.CssClass = "btn btn-success";
            }
           
            Controls.Add(btnSubmit);

            AddLiteral("</div>");

            
        }

        void OnAcceptClicked(object sender, EventArgs e)
        {
            EnsureChildControls();
            DataService.Instance.AcceptCookies();
            Page.Response.Redirect(Page.Request.Url.PathAndQuery);
        }
        
        void OnDeleteClicked(object sender, EventArgs e)
        {
            EnsureChildControls();
            DataService.Instance.DeleteCookies();
            Page.Response.Redirect(Page.Request.Url.PathAndQuery);
        }

        private void AddLiteral(string content)
        {
            Controls.Add(new LiteralControl(content));
        }
    }
}
