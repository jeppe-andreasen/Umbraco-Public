using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Xml;
using UmbracoPublic.Interfaces;

namespace UmbracoPublic.Logic.Providers
{
    public class YouTubeVideo : IVideo
    {
        public YouTubeVideo(YouTubeVideoProvider provider, XmlElement element)
        {
            Provider = provider;
            Id = element["id"].InnerText.Split('/').Last();
            Title = element["title"].InnerText;
        }

        public string Id { get; private set; }

        public string Title { get; private set; }

        public IVideoProvider Provider { get; private set; }

        public void Render(LinqIt.Utils.Web.HtmlWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Width, "100%");
            writer.AddAttribute(HtmlTextWriterAttribute.Height, "50");
            writer.AddAttribute(HtmlTextWriterAttribute.Src, "http://www.youtube.com/embed/" + Id);
            writer.AddAttribute("frameborder", "0");
            writer.RenderFullTag(HtmlTextWriterTag.Iframe, "");
        }
    }

}
