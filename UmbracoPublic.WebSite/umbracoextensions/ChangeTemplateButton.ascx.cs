using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Ajax;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using LinqIt.Components.Utilities;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.web;

namespace UmbracoPublic.WebSite.umbracoextensions
{
    public partial class ChangeTemplateButton : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxUtil.RegisterPageMethods(this);
        }

        [AjaxMethod(AjaxType.Sync)]
        public static void DoChangeDocumentType(string nodeId)
        {
            var documentType = DocumentType.GetByAlias("TwoColumnFrontpage");
            using (CmsContext.Editing)
            {
                var entity = CmsService.Instance.GetItem<Entity>(new Id(nodeId));
                var template = CmsService.Instance.GetTemplate(new Id(documentType.Id));
                CmsService.Instance.ChangeTemplate(entity, template);
            }
        }
    }
}