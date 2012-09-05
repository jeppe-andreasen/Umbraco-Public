using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using LinqIt.Components.Data;

namespace LinqIt.UmbracoCustomFieldTypes.Providers
{
    public class GoBasicImageEditorProvider : ImageEditorProvider
    {
        public override void GetImageProperties(string itemId, out string url, out string alternativeText)
        {
            url = null;
            alternativeText = null;
            using (CmsContext.Editing)
            {
                var image = CmsService.Instance.GetImage(new Id(itemId));
                if (image != null && image.Exists)
                {
                    url = image.Url;
                    alternativeText = image.AlternateText;
                }
            }
        }

        public override Type ImageTreeProviderType
        {
            get { return typeof (GoBasicImageTreeProvider); }
        }
    }
}
