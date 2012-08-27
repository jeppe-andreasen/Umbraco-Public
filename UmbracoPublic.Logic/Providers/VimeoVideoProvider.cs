using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using UmbracoPublic.Interfaces;
using UmbracoPublic.Logic.Modules.Video;

namespace UmbracoPublic.Logic.Providers
{
    public class VimeoVideoProvider : IVideoProvider
    {
        private readonly VimeoConfiguration _configuration;

        public VimeoVideoProvider(string referenceId)
        {
            var item = CmsService.Instance.GetItem<Entity>(new Id(referenceId));
            _configuration = CmsService.Instance.GetConfigurationItem<VimeoConfiguration>("VimeoConfiguration", item.Path);
        }

        public IEnumerable<IVideo> GetAllVideos()
        {
            if (_configuration != null)
            {
                if (!string.IsNullOrEmpty(_configuration.Username))
                    return GetVideosFromUrl(_configuration.Username);
                if (!string.IsNullOrEmpty(_configuration.AlbumId))
                    return GetVideosFromUrl("album/" + _configuration.AlbumId);
            }
            return new IVideo[0];
        }

        private IEnumerable<VimeoVideo> GetVideosFromUrl(string query)
        {
            var doc = new XmlDocument();
            var result = new List<VimeoVideo>();
            for (var i = 1; i <= 3; i++)
            {
                doc.Load("http://vimeo.com/api/v2/" + query + "/videos.xml?page=" + i);
                result.AddRange(from XmlElement element in doc.SelectNodes("videos/video") select new VimeoVideo(this, element));
                if (result.Count < i * 20)
                    return result;
            }
            return result;
        }

        public IVideo GetVideo(string id)
        {
            return GetAllVideos().Where(v => v.Id == id).FirstOrDefault();
        }
    }
}
