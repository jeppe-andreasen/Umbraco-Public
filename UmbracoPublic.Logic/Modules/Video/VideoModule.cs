using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;
using LinqIt.Components.Data;

namespace UmbracoPublic.Logic.Modules.Video
{
	public class VideoModule : BaseModule
	{
        public string VideoId { get { return GetValue<string>("video"); } }
	}
}
