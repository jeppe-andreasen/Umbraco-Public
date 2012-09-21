using System.Collections.Generic;
using System.Linq;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using umbraco.cms.businesslogic.web;

namespace UmbracoPublic.Logic.Entities
{
    public class CategorizationFolder : Entity
    {
        private CategorizationType[] _types;
        private Dictionary<Id, Categorization> _categorizations;

        public override Id TemplateId
        {
            get { return new Id(DocumentType.GetByAlias("CategorizationFolder").Id); }
        }

        public override string TemplatePath
        {
            get { return "/CategorizationFolder"; }
        }

        public static CategorizationFolder Get()
        {
            var path = CmsService.Instance.GetSystemPath("CategorizationFolder");
            if (string.IsNullOrEmpty(path))
                return null;
            return CmsService.Instance.GetItem<CategorizationFolder>(path);
        }

        public static CategorizationFolder Get(Id itemId)
        {
            using (CmsContext.Editing)
            {
                var entity = CmsService.Instance.GetItem<Entity>(itemId);
                var path = CmsService.Instance.GetSystemPath("CategorizationFolder", entity.Path);
                return CmsService.Instance.GetItem<CategorizationFolder>(path);
            }
        }

        public static Dictionary<Id, Categorization> GetVisibleCategorizations()
        {
            var categorizationLookup = Get();
            return categorizationLookup != null ? categorizationLookup.Types.Where(t => !t.IsHidden).SelectMany(t => t.Items).Where(i => !i.IsHidden).ToDictionary(i => i.Id) : new Dictionary<Id, Categorization>();
        }

        public CategorizationType[] Types
        {
            get { return _types ?? (_types = GetChildren<CategorizationType>().ToArray()); }
        }

        internal IEnumerable<CategorizationType> GetTypes(Id[] id)
        {
            return Types.Where(t => t.Items.Any(i => id.Contains(i.Id)));
        }

        public Categorization GetCategorization(Id id)
        {
            if (_categorizations == null)
                _categorizations = Types.SelectMany(t => t.Items).ToDictionary(i => i.Id);
            return _categorizations.ContainsKey(id) ? _categorizations[id] : null;
        }

        public bool HasCategorization(Id categorizationId)
        {
            return GetCategorization(categorizationId) != null;
        }

        
    }

    public class CategorizationType : Entity
    {
        private Categorization[] _items;

        public bool AllowMultipleSelections { get { return GetValue<bool>("allowMultipleSelections"); } }

        public bool Mandatory { get { return GetValue<bool>("mandatory"); } }

        public bool IsNewslistFilterOption { get { return GetValue<bool>("newslistFilterOption"); } }

        public bool IsHidden { get { return GetValue<bool>("hidden"); } }

        public Categorization[] Items
        {
            get { return _items ?? (_items = GetChildren<Categorization>().ToArray()); }
        }

        public bool HasItem(Id id)
        {
            return Items.Any(i => i.Id == id);
        }
    }
    
    public class Categorization : Entity
    {
        public string MailingListId { get { return GetValue<string>("mailingList"); } }
        public string MailingListName { get { return GetValue<string>("mailingListName"); } }
        public bool IsHidden { get { return GetValue<bool>("hidden"); } }
        public bool IsHiddenFromNewslistFilter
        {
            get
            {
                return IsHidden || GetValue<bool>("hideFromNewslistFilter");
            }
        }
    }

}
