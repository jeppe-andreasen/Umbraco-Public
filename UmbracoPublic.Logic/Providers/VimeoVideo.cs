using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Xml;
using UmbracoPublic.Interfaces;

namespace UmbracoPublic.Logic.Providers
{
    public class VimeoVideo : IVideo
    {
        public VimeoVideo(VimeoVideoProvider provider, XmlElement element)
        {
            Provider = provider;
            Id = element["id"].InnerText;
            Title = element["title"].InnerText;
            Url = element["url"].InnerText;
            Thumbnail = element["thumbnail_large"].InnerText;
            Width = Convert.ToInt32(element["width"].InnerText);
            Height = Convert.ToInt32(element["height"].InnerText);
        }

        public string Id { get; private set; }

        public string Title { get; private set; }

        public string Url { get; private set; }

        public string Thumbnail { get; private set; }

        public int Width { get; private set; }

        public int Height { get; private set; }

        public IVideoProvider Provider { get; private set; }

        public void Render(LinqIt.Utils.Web.HtmlWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Src, "http://player.vimeo.com/video/" + Id + "?title=0&amp;byline=0&amp;portrait=0&amp;color=ffffff");
            writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
            writer.AddAttribute(HtmlTextWriterAttribute.Height, "50");
            writer.AddAttribute("frameborder", "0");
            //writer.AddAttribute("webkitAllowFullScreen", "http://vimeo.com/37251159");
            writer.RenderFullTag(HtmlTextWriterTag.Iframe, "");
        }
    }
}
