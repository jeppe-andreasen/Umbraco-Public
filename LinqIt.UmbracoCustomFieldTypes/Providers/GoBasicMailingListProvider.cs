using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using LinqIt.Components.Data;
using UmbracoPublic.Logic.Entities;

namespace LinqIt.UmbracoCustomFieldTypes.Providers
{
    public class GoBasicMailingListProvider : NodeProvider
    {
        public GoBasicMailingListProvider(string referenceId) : base(referenceId)
        {
            
        }

        public override Node GetNode(string value)
        {
            return GetRootNodes().Where(n => n.Id == value).FirstOrDefault();
        }

        public override IEnumerable<Node> GetRootNodes()
        {
            using (CmsContext.Editing)
            {
                var referenceItem = CmsService.Instance.GetItem<Entity>(new Id(_referenceId));
                var mailConfiguration = CmsService.Instance.GetConfigurationItem<MailConfiguration>("Mail", referenceItem.Path);
                if (mailConfiguration == null)
                    throw new ConfigurationErrorsException("Mail configuration not found");
                return mailConfiguration.MailProvider.GetLists().Select(l => new Node(){Id = l.Id, Text = l.DisplayName});
            }
        }
    }
}
