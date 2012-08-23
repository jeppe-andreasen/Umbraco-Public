using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Components;

namespace UmbracoPublic.Logic.Providers
{
    public class SiteRootLayoutProvider : IGridModuleControl
    {
        public GridLayout GetGridLayout()
        {
            var result = new GridLayout(12);
            result.AddRow().AddCell(12, "Footer", "Footer", GridLayoutCellType.Placeholder);
            return result;
        }
    }
}
