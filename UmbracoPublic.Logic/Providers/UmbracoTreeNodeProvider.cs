using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using LinqIt.Components.Data;

namespace UmbracoPublic.Logic.Providers
{
    public abstract class UmbracoTreeNodeProvider : TreeNodeProvider
    {
        protected UmbracoTreeNodeProvider(string referenceId) : base(referenceId)
        {
            
        }

        protected abstract string GetRootPath();

        protected abstract bool IsSelectable(Entity entity);

        public override IEnumerable<Node> GetChildNodes(Node node)
        {
            using (CmsContext.Editing)
            {
                var entity = CmsService.Instance.GetItem<Entity>(new Id(node.Id));
                return entity.GetChildren<Entity>().Select(GetNode);
            }
        }

        public override Node GetParentNode(Node node)
        {
            using (CmsContext.Editing)
            {
                var entity = CmsService.Instance.GetItem<Entity>(new Id(node.Id));
                
                var folderPath = GetRootPath();
                if (entity.Path == folderPath)
                    return null;

                return GetNode(entity.GetParent<Entity>());
            }
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

        public override IEnumerable<Node> GetRootNodes()
        {
            using (CmsContext.Editing)
            {
                var folderPath = GetRootPath();
                var folder = CmsService.Instance.GetItem<Entity>(folderPath);
                return folder.GetChildren<Entity>().Select(GetNode);
            }
        }

        private Node GetNode(Entity entity)
        {
            if (entity == null)
                return null;

            var result = new Node();
            result.HelpText = entity.DisplayName;
            result.Icon = entity.Icon;
            result.Selectable = IsSelectable(entity);
            result.Id = entity.Id.ToString();
            result.Text = entity.DisplayName;
            result.Draggable = true;
            return result;
        }

    }
}
