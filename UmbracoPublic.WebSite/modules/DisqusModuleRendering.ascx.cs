using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Cms;
using LinqIt.Utils.Web;
using UmbracoPublic.Logic.Entities;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.WebSite.modules
{
    public partial class DisqusModuleRendering : BaseModule<DisqusModule>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var configuration = CmsService.Instance.GetConfigurationItem<DisqusConfiguration>("DisqusConfiguration");
            var script = string.Format("application.disqus.init({0},'{1}');", configuration.DeveloperMode ? "1" : "0", configuration.SiteName);
            ModuleScripts.Instance.RegisterInitializationScripts(this, script);
        }

        protected override void RenderModule(DisqusModule module, LinqIt.Utils.Web.HtmlWriter writer)
        {

        }
    }
}