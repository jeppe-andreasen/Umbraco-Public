using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Cms;
using LinqIt.Components;
using LinqIt.Utils.Web;
using UmbracoPublic.Logic.Utilities;
using UmbracoPublic.WebSite.Utilities;

namespace UmbracoPublic.WebSite.masterpages
{
    public partial class GeneralPage : System.Web.UI.MasterPage, IGridModuleControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public GridLayout GetGridLayout()
        {
            return PageLayouts.GeneralLayout;
        }
    }
}