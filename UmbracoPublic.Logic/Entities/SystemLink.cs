using LinqIt.Cms.Data;
using umbraco.cms.businesslogic.web;

namespace UmbracoPublic.Logic.Entities
{
    public class SystemLink : Entity
    {
        public override Id TemplateId
        {
            get { return new Id(DocumentType.GetByAlias("SystemLink").Id); }
        }

        public override string TemplatePath
        {
            get { return "/SystemLink"; }
        }

        public Id Link
        {
            get { return GetValue<Id>("link");}
            set { SetValue("link", value); }
        }
    }
}
