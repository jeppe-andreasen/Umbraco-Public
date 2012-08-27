using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;
using LinqIt.Components.Data;

namespace UmbracoPublic.Logic.Modules.ImageGallery
{
	public class ImageGalleryModule : BaseModule
	{
	    public ImageGalleryData Data
	    {
	        get { return ImageGalleryData.Parse(this["imageGalleryContent"]); }
	    }
	}
}
