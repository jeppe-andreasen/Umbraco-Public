using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;

namespace UmbracoPublic.Logic.Entities
{
    public class GridModule : Entity
    {
        public bool Highlighted { get { return GetValue<bool>("highlighted"); } }
    }
}
