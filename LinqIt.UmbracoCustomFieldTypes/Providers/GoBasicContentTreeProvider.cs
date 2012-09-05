using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms;
using LinqIt.Cms.Data;

namespace LinqIt.UmbracoCustomFieldTypes.Providers
{
    public class GoBasicContentTreeProvider : EntityTreeNodeProvider
    {
        public GoBasicContentTreeProvider(string referenceId) : base(referenceId)
        {
        }

        public override IEnumerable<LinqIt.Components.Data.Node> GetRootNodes()
        {
            using (CmsContext.Editing)
            {
                var homeItem = CmsService.Instance.GetHomeItem();
                if (homeItem == null)
                {
                    var referenceItem = CmsService.Instance.GetItem<Entity>(new Id(_referenceId));
                    var systemPath = CmsService.Instance.GetSystemPath("HomePage", referenceItem.Path);
                    homeItem = CmsService.Instance.GetItem<Page>(systemPath);
                }
                return new[] { GetNode(homeItem) };
            }
        }
    }
}
