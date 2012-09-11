using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.UI;
using LinqIt.Ajax.Parsing;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.Logic.Modules.Disqus
{
    public class DisqusModuleRendering : BaseModuleRendering<DisqusModule>, IRequiresCookies
    {
        protected override void RegisterScripts()
        {
            var configuration = GetRequiredConfig<DisqusConfiguration>("Disqus");
            ModuleScripts.RegisterInitScript("disqus", new JSONBoolean(configuration.DeveloperMode), new JSONString(configuration.SiteName));
        }

        protected override void RenderModule(DisqusModule module, LinqIt.Utils.Web.HtmlWriter writer)
        {
            writer.Write("Diskuter");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "disqus_thread");
            writer.RenderFullTag(HtmlTextWriterTag.Div, "");
        }

        public override string ModuleDescription
        {
            get { return "Kommentarboks"; }
        }
    }
}
