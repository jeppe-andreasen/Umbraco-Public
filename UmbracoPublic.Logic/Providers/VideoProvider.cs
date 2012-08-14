using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.UI;
using LinqIt.Components.Data;
using LinqIt.Utils;
using LinqIt.Utils.Extensions;
using LinqIt.Utils.Web;
using UmbracoPublic.Interfaces;

namespace UmbracoPublic.Logic.Providers
{
    public class VideoProvider : NodeProvider
    {
        public VideoProvider(string referenceId) : base(referenceId)
        {
        }

        public override IEnumerable<Node> GetRootNodes()
        {
            var providerTypes = new[] { typeof(YouTubeVideoProvider), typeof(VimeoVideoProvider) };
            var videos = new List<IVideo>();
            foreach (var provider in providerTypes.Select(type => ProviderHelper.GetProvider<IVideoProvider>(type.AssemblyQualifiedName, _referenceId)))
            {
                videos.AddRange(provider.GetAllVideos());
            }
            return videos.OrderBy(v => v.Title).Select(GetVideoNode);
        }

        private IVideo GetVideo(string value)
        {
            var parts = value.Split('|');
            var providerType = parts[0];
            var id = parts[1];

            var provider = ProviderHelper.GetProvider<IVideoProvider>(providerType, _referenceId);
            return provider.GetVideo(id);
        }

        private static Node GetVideoNode(IVideo video)
        {
            var node = new Node();
            node.Id = video.Provider.GetType().GetShortAssemblyName() + "|" + video.Id;
            node.Text = video.Title;
            return node;
        }

        public override Node GetNode(string value)
        {
            var video = GetVideo(value);
            return video != null ? GetVideoNode(video) : null;
        }

        public void RenderVideoModule(HtmlWriter writer, string videoId)
        {
            var video = GetVideo(videoId);
            if (video != null)
                video.Render(writer);
        }
    }
}
