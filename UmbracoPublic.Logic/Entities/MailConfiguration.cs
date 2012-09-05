using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;
using UmbracoPublic.Interfaces;

namespace UmbracoPublic.Logic.Entities
{
    public class MailConfiguration : Entity
    {
        public IMailProvider MailProvider
        {
            get 
            { 
                var typeName = this.GetValue<string>("mailService");
                if (string.IsNullOrEmpty(typeName))
                    throw new ConfigurationErrorsException("A mail service as not been configured");
                var type = Type.GetType(typeName);
                if (type == null)
                    throw new ArgumentException("Could not load Mail Service Type: " + typeName);

                return (IMailProvider)Activator.CreateInstance(type);
            }
        }
    }
}
