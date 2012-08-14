using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using LinqIt.Utils.Web;
using UmbracoPublic.Logic.Entities;
using UmbracoPublic.Logic.Providers;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.WebSite.modules
{
    public partial class VideoModuleRendering : BaseModule<VideoModule>
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterInitScript("videomodule");
        }

        protected override void RenderModule(VideoModule module, LinqIt.Utils.Web.HtmlWriter writer)
        {
            var page = CmsService.Instance.GetItem<Entity>();
            var videoProvider = new VideoProvider(page.Id.ToString());
            writer.RenderBeginTag(HtmlTextWriterTag.Div, "module video-module");
            videoProvider.RenderVideoModule(writer, module.VideoId);
            writer.RenderEndTag();
        }
    }
}