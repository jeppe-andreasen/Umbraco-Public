using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms;
using LinqIt.Cms.Data;

namespace UmbracoPublic.Logic.Entities
{
    public class CategorizationFolder : Entity
    {
        private CategorizationType[] _types;
        private Dictionary<Id, Categorization> _categorizations;

        public static CategorizationFolder Get()
        {
            var path = CmsService.Instance.GetSystemPath("CategorizationFolder");
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

        public CategorizationType[] Types
        {
            get { return _types ?? (_types = GetChildren<CategorizationType>().ToArray()); }
        }

        internal IEnumerable<CategorizationType> GetTypes(LinqIt.Cms.Data.Id[] id)
        {
            return Types.Where(t => t.Items.Where(i => id.Contains(i.Id)).Any());
        }

        public Categorization GetCategorization(LinqIt.Cms.Data.Id id)
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
            return Items.Where(i => i.Id == id).Any();
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
