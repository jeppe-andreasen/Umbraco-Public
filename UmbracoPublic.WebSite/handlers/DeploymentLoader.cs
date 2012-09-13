using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using umbraco.cms.presentation.Trees;

namespace UmbracoPublic.WebSite.handlers
{
    public class DeploymentLoader : BaseTree
    {
        public DeploymentLoader(string application) : base(application)
        {
            
        }

        protected override void CreateRootNode(ref XmlTreeNode rootNode)
        {
            rootNode.Icon = FolderIcon;
            rootNode.OpenIcon = FolderIconOpen;
            rootNode.NodeType = "init" + TreeAlias;
            rootNode.NodeID = "init";
        }

        /// 
        /// Override the render method to create the newsletter tree
        /// 
        /// 
        public override void Render(ref XmlTree Tree)
        {
            // Create tree node to allow sending a newsletter
            var createDeploymentPackage = XmlTreeNode.Create(this);
            createDeploymentPackage.Text = "Create Deployment Package";
            createDeploymentPackage.Icon = "docPic.gif";
            createDeploymentPackage.Action = "javascript:openCreateDeploymentPackage()";
            // Add the node to the tree
            Tree.Add(createDeploymentPackage);

            // Create tree node to allow viewing previously sent newsletters
            var installDeploymentPackage = XmlTreeNode.Create(this);
            installDeploymentPackage.Text = "Install Deployment Package";
            installDeploymentPackage.Icon = "docPic.gif";
            installDeploymentPackage.Action = "javascript:openInstallDeploymentPackage()";
            // Add the node to the tree
            Tree.Add(installDeploymentPackage);
        }

        public override void RenderJS(ref StringBuilder Javascript)
        {
            Javascript.Append(@"
                function openCreateDeploymentPackage() {
                    parent.right.document.location.href = '/handlers/BuildPackage.aspx';
                }
			");

            Javascript.Append(@"
                function openInstallDeploymentPackage() {
                    parent.right.document.location.href = '/handlers/InstallPackage.aspx';
                }
			");
        }
    }
}