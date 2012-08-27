using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;

namespace UmbracoPublic.Logic.Modules.Video
{
    public class VimeoConfiguration : Entity
    {
        public string Username { get { return GetValue<string>("username"); } }

        public string AlbumId { get { return GetValue<string>("albumId"); } }
    }
}
