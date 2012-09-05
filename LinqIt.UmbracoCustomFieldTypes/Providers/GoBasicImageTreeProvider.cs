using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqIt.UmbracoCustomFieldTypes.Providers
{
    public class GoBasicImageTreeProvider: UmbracoMediaTreeProvider
    {
        public GoBasicImageTreeProvider(string referenceId) : base(referenceId)
        {
            
        }

        protected override bool IsSelectable(umbraco.cms.businesslogic.media.Media media)
        {
            return media.ContentType.Alias == "Image";
        }
    }
}