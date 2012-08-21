using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using LinqIt.Components.Data;

namespace UmbracoPublic.Logic.Providers
{
    public class CategorizationProvider : UmbracoTreeNodeProvider
    {
        public CategorizationProvider(string referenceId) : base(referenceId)
        {
        }

        protected override string GetRootPath()
        {
            using (CmsContext.Editing)
            {
                var currentItem = CmsService.Instance.GetItem<Entity>(new Id(_referenceId));
                return currentItem.Path;
            }
        }

        protected override bool IsSelectable(Entity entity)
        {
            return entity.Template.Name == "Categorization";
        }
    }
}
