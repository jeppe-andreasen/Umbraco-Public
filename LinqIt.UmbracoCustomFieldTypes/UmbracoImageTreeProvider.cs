using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using LinqIt.Components.Data;
using LinqIt.UmbracoServices.Data;

namespace LinqIt.UmbracoCustomFieldTypes
{
    public class UmbracoImageTreeProvider : UmbracoMediaTreeProvider
    {
        public UmbracoImageTreeProvider(string referenceId) : base(referenceId)
        {
            
        }

        protected override bool IsSelectable(umbraco.cms.businesslogic.media.Media media)
        {
            return media.ContentType.Alias == "Image";
        }
    }
}
