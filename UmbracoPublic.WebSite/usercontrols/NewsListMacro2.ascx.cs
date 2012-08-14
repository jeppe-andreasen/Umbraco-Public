using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UmbracoPublic.WebSite.usercontrols
{
    public partial class NewsListMacro2 : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public int NumberOfItemsToShow { get; set; }
    }
}