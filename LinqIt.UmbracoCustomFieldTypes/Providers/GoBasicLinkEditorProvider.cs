using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using LinqIt.Components.Data;
using umbraco.cms.businesslogic.media;

namespace LinqIt.UmbracoCustomFieldTypes.Providers
{
    public class GoBasicLinkEditorProvider : LinkEditorProvider
    {
        protected override string GetInternalUrl(string itemId)
        {
            using (CmsContext.Editing)
            {
                var page = CmsService.Instance.GetItem<Page>(new Id(itemId));
                return page == null ? string.Empty : page.Url;
            }
        }

        protected override string GetMediaUrl(string itemId)
        {
            if (string.IsNullOrEmpty(itemId))
                return string.Empty;
            var media = new Media(Convert.ToInt32(itemId));
            return (string) media.getProperty("umbracoFile").Value;
        }

        public override Type InternalTreeProviderType
        {
            get { return typeof (GoBasicContentTreeProvider); }
        }

        public override Type MediaTreeProviderType
        {
            get { return typeof (GoBasicMediaTreeProvider); }
        }
    }
}
