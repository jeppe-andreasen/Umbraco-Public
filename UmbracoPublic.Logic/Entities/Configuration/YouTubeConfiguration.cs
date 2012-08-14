using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;

namespace UmbracoPublic.Logic.Entities.Configuration
{
    public class YouTubeConfiguration : Entity
    {
        public string Username { get { return GetValue<string>("username"); } }
    }
}
