using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Components;
using UmbracoPublic.WebSite.Utilities;

namespace UmbracoPublic.WebSite.masterpages
{
    public partial class OneColumnWidePage : System.Web.UI.MasterPage, IGridModuleControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public GridLayout GetGridLayout()
        {
            return PageLayouts.WidePage1Column;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

        }
    }
}