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
        private RadioButton _rbAccepted;
        private RadioButton _rbNotAccepted;

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            AddLiteral("<fieldset>");
            AddLiteral("<legend>Cookies</legend>");
            AddLiteral("<label>Accepter brugen af cookies på dette website</label>");
            var cookieState = DataService.Instance.GetCookieState();
            _rbAccepted = new RadioButton();
            _rbAccepted.ID = "rbAccepted";
            _rbAccepted.GroupName = "CookieState";
            _rbAccepted.Checked = cookieState == CookieState.Accepted;
            _rbAccepted.Text = "Jeg accepterer";
            Controls.Add(_rbAccepted);

            _rbNotAccepted = new RadioButton();
            _rbNotAccepted.ID = "rbNotAccepted";
            _rbNotAccepted.GroupName = "CookieState";
            _rbNotAccepted.Checked = cookieState == CookieState.NotAccepted;
            _rbNotAccepted.Text = "Jeg accepterer ikke";
            Controls.Add(_rbNotAccepted);

            var btnSubmit = new Button();
            btnSubmit.CssClass = "btn btn-submit";
            btnSubmit.Click += OnSubmit;

            AddLiteral("</fieldset>");
        }

        void OnSubmit(object sender, EventArgs e)
        {
            EnsureChildControls();

            if (_rbAccepted.Checked)
                DataService.Instance.SetCookieState(CookieState.Accepted);
            else
                DataService.Instance.SetCookieState(CookieState.NotAccepted);
        }

        private void AddLiteral(string content)
        {
            Controls.Add(new LiteralControl(content));
        }
    }
}
