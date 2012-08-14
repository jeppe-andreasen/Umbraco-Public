using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using LinqIt.Components.Data;

namespace UmbracoPublic.Logic.Providers
{
    public class SubjectProvider : NodeProvider
    {
        public SubjectProvider(string referenceId) : base(referenceId)
        {
            
        }

        public override IEnumerable<Node> GetRootNodes()
        {
            using (CmsContext.Editing)
            {
                var currentItem = CmsService.Instance.GetItem<Entity>(new Id(_referenceId));
                var folderPath = CmsService.Instance.GetSystemPath("SubjectFolder", currentItem.Path);
                var folder = CmsService.Instance.GetItem<Entity>(folderPath);
                return folder.GetChildren<Entity>().Select(GetNode);
            }
        }

        private static Node GetNode(Entity entity)
        {
            if (entity == null)
                return null;

            var result = new Node();
            result.HelpText = entity.DisplayName;
            result.Icon = entity.Icon;
            result.Selectable = true;
            result.Id = entity.Id.ToString();
            result.Text = entity.DisplayName;
            result.Draggable = true;
            return result;
        }

        public override Node GetNode(string value)
        {
            using (CmsContext.Editing)
            {
                var id = new Id(value);
                var entity = CmsService.Instance.GetItem<Entity>(id);
                return GetNode(entity);
            }
        }
    }
}
