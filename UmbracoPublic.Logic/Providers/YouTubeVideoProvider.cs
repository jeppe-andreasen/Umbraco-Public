using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using UmbracoPublic.Interfaces;
using UmbracoPublic.Logic.Entities.Configuration;

namespace UmbracoPublic.Logic.Providers
{
    public class YouTubeVideoProvider : IVideoProvider
    {
        private readonly YouTubeConfiguration _configuration;

        public YouTubeVideoProvider(string referenceId)
        {
            var item = CmsService.Instance.GetItem<Entity>(new Id(referenceId));
            _configuration = CmsService.Instance.GetConfigurationItem<YouTubeConfiguration>("YouTubeConfiguration", item.Path);
        }

        private List<IVideo> _videos;

        public IEnumerable<IVideo> GetAllVideos()
        {
            if (_configuration == null || string.IsNullOrEmpty(_configuration.Username))
                return null;

            if (_videos == null)
            {
                _videos = new List<IVideo>();
                var client = new WebClient();
                var xml = client.DownloadString("https://gdata.youtube.com/feeds/api/videos?author=" + _configuration.Username);
                var doc = new XmlDocument();
                doc.LoadXml(xml);

                foreach (XmlElement element in doc.DocumentElement.ChildNodes.Cast<XmlNode>().Where(n => n.Name == "entry").Cast<XmlElement>())
                {
                    _videos.Add(new YouTubeVideo(this, element));
                }
            }
            return _videos;
        }


        public IVideo GetVideo(string id)
        {
            return GetAllVideos().Where(v => v.Id == id).FirstOrDefault();
        }
    }
}
