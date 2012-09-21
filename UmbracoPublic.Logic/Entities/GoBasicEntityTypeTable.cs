using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;
using UmbracoPublic.Logic.Modules.Forms;
using UmbracoPublic.Logic.Services.Newsletters.MailChimp;

namespace UmbracoPublic.Logic.Entities
{
    public class GoBasicEntityTypeTable : EntityTypeTable
    {
        public GoBasicEntityTypeTable()
        {
            this.AddType("/ConfigurationFolder", typeof(ConfigurationFolder));
            this.AddType("/CategorizationFolder", typeof(CategorizationFolder));
            this.AddType("/ContentFolder", typeof(ContentFolder));
            this.AddType("/SystemLinkFolder", typeof(SystemLinkFolder));
            this.AddType("/SystemLink", typeof(SystemLink));
            this.AddType("/Configuration/ServiceMenuConfiguration", typeof(ServiceMenuConfiguration));
            this.AddType("/Configuration/NewsletterConfiguration", typeof(NewsletterConfiguration));
            this.AddType("/Configuration/MailChimpConfiguration", typeof (MailChimpConfiguration));
            this.AddType("/GridModuleFolder", typeof(GridModuleFolder));
            this.AddType("/GlobalGridModuleFolder", typeof(GlobalGridModuleFolder));

            this.AddType("/FormsFieldFolder", typeof(FormsFieldFolder));
            
            this.AddType("/FormsField/FormsTextBoxField", typeof(FormsTextBoxField));
            this.AddType("/FormsField/FormsDropDownField", typeof(FormsDropDownField));
            this.AddType("/FormsField/FormsCheckBoxField", typeof(FormsCheckBoxField));
            this.AddType("/FormsField/FormsFileUploadField", typeof(FormsFileUploadField));
            this.AddType("/FormsField/FormsEmailField", typeof(FormsEmailField));

            this.AddType("/FormsActionFolder", typeof(FormsActionFolder));
            this.AddType("/FormsAction", typeof(FormsAction));
            this.AddType("/FormsAction/FormsSendMailAction", typeof (FormsSendMailAction));

            this.AddType("/WebPage", typeof(WebPage));
            this.AddType("/WebPage/SiteSearchResultPage", typeof(SiteSearchResultPage));
            this.AddType("/WebPage/NewsListPage", typeof(NewsListPage));
        }
    }
}
