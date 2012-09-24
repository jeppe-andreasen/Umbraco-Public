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
            return GetRootNodes().FirstOrDefault(n => n.Id == value);
        }

        public override IEnumerable<Node> GetRootNodes()
        {
            using (CmsContext.Editing)
            {
                var referenceItem = CmsService.Instance.GetItem<Entity>(new Id(_referenceId));
                var mailConfiguration = CmsService.Instance.GetConfigurationItem<NewsletterConfiguration>("Mail", referenceItem.Path);
                if (mailConfiguration == null)
                    return new Node[0];
                var newsletterService = mailConfiguration.NewsletterService;
                return newsletterService != null ? newsletterService.GetLists().Select(l => new Node(){Id = l.Id, Text = l.DisplayName}) : new Node[0];
            }
        }
    }
}
