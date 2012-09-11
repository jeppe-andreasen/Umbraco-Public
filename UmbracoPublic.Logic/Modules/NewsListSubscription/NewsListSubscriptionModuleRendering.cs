using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using UmbracoPublic.Logic.Entities;
using UmbracoPublic.Logic.Services;

namespace UmbracoPublic.Logic.Modules.NewsListSubscription
{
    public class NewsListSubscriptionModuleRendering : BaseModuleRendering<NewsListSubscriptionModule>
    {
        private CheckBoxList _cblSubscriptions;
        private TextBox _txtEmailAddress;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ViewStateMode = ViewStateMode.Enabled;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            var categorizations = CategorizationFolder.Get();
            var items = categorizations.Types.SelectMany(t => t.Items).Where(i => !string.IsNullOrEmpty(i.MailingListId)).ToArray();
            _cblSubscriptions = new CheckBoxList();
            _cblSubscriptions.ID = "clbSubscriptions";
            foreach (var item in items)
            {
                string name = !string.IsNullOrEmpty(item.MailingListName) ? item.MailingListName : item.DisplayName;
                var value = item.MailingListId;
                _cblSubscriptions.Items.Add(new ListItem(name, value));
            }
            Controls.Add(_cblSubscriptions);

            var label = new Label();
            label.Text = "Email address:";
            label.ID = "lblEmailAddress";
            label.AssociatedControlID = "txtEmailAddress";
            Controls.Add(label);

            _txtEmailAddress = new TextBox();
            _txtEmailAddress.ID = "txtEmailAddress";
            Controls.Add(_txtEmailAddress);

            var button = new Button();
            button.ID = "btnUpdateSubscriptions";
            button.Click += OnUpdateSubscriptionsClicked;
            button.Text = "Tilmeld nyhedslister";
            Controls.Add(button);
        }

        void OnUpdateSubscriptionsClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_txtEmailAddress.Text))
            {
                foreach (var item in _cblSubscriptions.Items.Cast<ListItem>().Where(li => li.Selected))
                    DataService.Instance.SubscribeToNewsletter(_txtEmailAddress.Text, item.Value);
            }
        }

        public override string ModuleDescription
        {
            get { return "Nyhedsliste tilmelding"; }
        }
    }
}
