using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Components.Data;
using LinqIt.Utils;
using LinqIt.Utils.Extensions;

namespace LinqIt.UmbracoCustomFieldTypes.Providers
{
    public abstract class GoBasicImplementationProvider<T> : NodeProvider
    {
        public override Node GetNode(string value)
        {
            return GetRootNodes().Where(n => n.Id == value).FirstOrDefault();
        }

        public override IEnumerable<Node> GetRootNodes()
        {
            foreach (var type in TypeUtility.GetTypesImplementingInterface<T>(AppDomain.CurrentDomain))
            {
                var node = new Node();
                node.Id = type.GetShortAssemblyName();
                node.Text = type.FullName;
                yield return node;
            }
        }
    }
}
