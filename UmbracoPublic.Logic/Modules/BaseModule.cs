using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;

namespace UmbracoPublic.Logic.Modules
{
    public class BaseModule : Entity
    {
        public bool Highlighted { get { return GetValue<bool>("highlighted"); } }
    }
}
