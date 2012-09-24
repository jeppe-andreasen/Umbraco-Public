using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;
using UmbracoPublic.Interfaces;
using umbraco.cms.businesslogic.web;

namespace UmbracoPublic.Logic.Entities
{
    public class NewsletterConfiguration : Entity
    {
        public override Id TemplateId
        {
            get
            {
                return new Id(DocumentType.GetByAlias("NewsletterConfiguration").Id);
            }
        }

        public override string TemplatePath
        {
            get { return "/Configuration/NewsletterConfiguration"; }
        }

        public INewsletterService NewsletterService
        {
            get 
            { 
                var typeName = this.GetValue<string>("newsletterService");
                if (string.IsNullOrEmpty(typeName))
                    return null;
                var type = Type.GetType(typeName);
                if (type == null)
                    return null;

                return (INewsletterService)Activator.CreateInstance(type);
            }
        }
    }
}
