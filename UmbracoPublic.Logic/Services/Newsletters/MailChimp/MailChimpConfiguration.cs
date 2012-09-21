using LinqIt.Cms.Data;
using umbraco.cms.businesslogic.web;

namespace UmbracoPublic.Logic.Services.Newsletters.MailChimp
{
    public class MailChimpConfiguration : Entity
    {
        public string ApiKey
        {
            get { return GetValue<string>("apiKey"); }
        }

        public override Id TemplateId
        {
            get
            {
                return new Id(DocumentType.GetByAlias("MailChimpConfiguration").Id);
            }
        }

        public override string TemplatePath
        {
            get { return "/Configuration/MailChimpConfiguration"; }
        }
    }
}
