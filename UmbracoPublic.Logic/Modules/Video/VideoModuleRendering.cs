using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using UmbracoPublic.Logic.Providers;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.Logic.Modules.Video
{
    public class VideoModuleRendering : BaseModuleRendering<VideoModule>
    {
        protected override void RegisterScripts()
        {
            ModuleScripts.RegisterInitScript("videomodule");
        }

        protected override void RenderModule(VideoModule module, LinqIt.Utils.Web.HtmlWriter writer)
        {
            var page = CmsService.Instance.GetItem<Entity>();
            var videoProvider = new VideoProvider(page.Id.ToString());
            writer.RenderBeginTag(HtmlTextWriterTag.Div, "video-module");
            videoProvider.RenderVideoModule(writer, module.VideoId);
            writer.RenderEndTag();
        }

        public override string ModuleDescription
        {
            get { return "Video boks"; }
        }
    }
}
