using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UmbracoPublic.Interfaces
{
    public interface IVideoProvider
    {
        IEnumerable<IVideo> GetAllVideos();

        IVideo GetVideo(string id);
    }
}
