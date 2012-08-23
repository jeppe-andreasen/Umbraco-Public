using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using LinqIt.Components.Data;

namespace LinqIt.UmbracoCustomFieldTypes
{
    public class ImageGalleryProvider: TreeNodeProvider
    {
        public ImageGalleryProvider(string referenceId) : base(referenceId)
        {
            
        }

        public override IEnumerable<Node> GetRootNodes()
        {
            return new [] {RootNode};
        }

        private ImageGalleryData GetData()
        {
            return (ImageGalleryData)HttpContext.Current.Session["ImageGalleryData_" + _referenceId];
        }

        public override IEnumerable<Node> GetChildNodes(Node node)
        {
            return node.Id == HttpContext.Current.Request.QueryString["itemId"] ? (IEnumerable<Node>) GetData().Items : new Node[0];
        }

        public override Node GetNode(string value)
        {
            return value == HttpContext.Current.Request.QueryString["itemId"] ? RootNode : GetData().Items.Where(i => i.Id == value).FirstOrDefault();
        }

        public override Node GetParentNode(Node node)
        {
            return node.Id == HttpContext.Current.Request.QueryString["itemId"] ? null: RootNode;
        }

        private static Node RootNode
        {
            get
            {
                using (CmsContext.Editing)
                {
                    var root = CmsService.Instance.GetItem<Entity>(new Id(HttpContext.Current.Request.QueryString["itemId"]));
                    var rootNode = new Node();
                    rootNode.Id = root.Id.ToString();
                    rootNode.Text = root.EntityName;
                    rootNode.Icon = root.Icon;
                    rootNode.Selectable = false;
                    rootNode.Draggable = false;
                    return rootNode;
                }
            }
        }
    }
}
