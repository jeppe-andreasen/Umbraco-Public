using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using LinqIt.Cms;
using LinqIt.Components.Data;
using LinqIt.UmbracoServices.Data;

namespace LinqIt.UmbracoCustomFieldTypes
{
    public class UmbracoMediaTreeProvider : TreeNodeProvider
    {
        public UmbracoMediaTreeProvider(string referenceId) : base(referenceId)
        {
        }

        public override IEnumerable<Node> GetChildNodes(Node node)
        {
            var media = new umbraco.cms.businesslogic.media.Media(Convert.ToInt32(node.Id));
            return media.Children.Select(GetNode);
        }

        public override Node GetParentNode(Node node)
        {
            var media = new umbraco.cms.businesslogic.media.Media(Convert.ToInt32(node.Id));
            if (media.ParentId == -1)
                return null;
            return GetNode(media.ParentId.ToString());
        }

        public override Node GetNode(string value)
        {
            var media = new umbraco.cms.businesslogic.media.Media(Convert.ToInt32(value));
            return GetNode(media);
        }

        public override IEnumerable<Node> GetRootNodes()
        {
            var context = new UmbracoDataContext(ConfigurationManager.AppSettings["umbracoDbDSN"]);
            var nodeObjectType = new Guid("B796F64C-1F99-4FFB-B886-4BF4BC011A9C");
            var rootNodes = context.umbracoNodes.Where(n => n.parentID == -1 && n.nodeObjectType == nodeObjectType).OrderBy(n => n.sortOrder).ToArray();
            using (CmsContext.Editing)
            {
                return rootNodes.Select(n => GetNode(new umbraco.cms.businesslogic.media.Media(n.id)));
            }
        }

        private Node GetNode(umbraco.cms.businesslogic.media.Media media)
        {
            var result = new Node();
            result.Id = media.Id.ToString();
            result.Icon = "/umbraco/images/umbraco/" + media.ContentTypeIcon;
            result.Draggable = false;
            result.HelpText = media.Text;
            result.Text = media.Text;
            result.Selectable = IsSelectable(media);
            return result;
        }

        protected virtual bool IsSelectable(umbraco.cms.businesslogic.media.Media media)
        {
            return true;
        }
    }
}
