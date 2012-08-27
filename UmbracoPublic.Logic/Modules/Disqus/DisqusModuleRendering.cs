using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using LinqIt.Ajax.Parsing;
using LinqIt.Cms;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.Logic.Modules.Disqus
{
    public class DisqusModuleRendering : BaseModuleRendering<DisqusModule>
    {
        protected override void OnRegisterScripts()
        {
            var configuration = CmsService.Instance.GetConfigurationItem<DisqusConfiguration>("DisqusConfiguration");
            ModuleScripts.RegisterInitScript("disqus", new JSONBoolean(configuration.DeveloperMode), new JSONString(configuration.SiteName));
        }

        protected override void RenderModule(DisqusModule module, LinqIt.Utils.Web.HtmlWriter writer)
        {
            writer.Write("Diskuter");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "disqus_thread");
            writer.RenderFullTag(HtmlTextWriterTag.Div, "");
        }
    }
}
