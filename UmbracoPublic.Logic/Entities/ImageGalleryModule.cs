using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;
using LinqIt.Components.Data;
namespace UmbracoPublic.Logic.Entities
{
	public class ImageGalleryModule : Entity
	{
	    public ImageGalleryData Data
	    {
	        get { return ImageGalleryData.Parse(this["imageGalleryContent"]); }
	    }
	}
}
