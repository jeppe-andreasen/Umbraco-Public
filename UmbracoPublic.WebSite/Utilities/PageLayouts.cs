using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LinqIt.Components;

namespace UmbracoPublic.WebSite.Utilities
{
    public class PageLayouts
    {
        public static LinqIt.Components.GridLayout GeneralLayout
        {
            get
            {
                var result = new GridLayout(12);
                var mainRow = result.AddRow();
                mainRow.AddCell(3, "Left Menu", "Left Menu", GridLayoutCellType.ContentBlock);
                var innerCell = mainRow.AddGridCell(9);
                innerCell.AddRow().AddCell(9, "Top", "Top", GridLayoutCellType.Placeholder);
                var contentRow = innerCell.AddRow();
                contentRow.AddCell(6, "Main", "Main", GridLayoutCellType.Placeholder);
                contentRow.AddCell(3, "Right", "Right", GridLayoutCellType.Placeholder);
                innerCell.AddRow().AddCell(9, "Bottom", "Bottom", GridLayoutCellType.Placeholder);
                return result;
            }
        }

        public static LinqIt.Components.GridLayout GeneralWideTopLayout
        {
            get
            {
                var result = new GridLayout(12);
                result.AddRow().AddCell(12, "Top", "Top", GridLayoutCellType.Placeholder);
                var contentRow = result.AddRow();
                contentRow.AddCell(3, "Menu", "Menu", GridLayoutCellType.ContentBlock);
                contentRow.AddCell(6, "Main", "Main", GridLayoutCellType.Placeholder);
                contentRow.AddCell(3, "Right", "Right", GridLayoutCellType.Placeholder);
                return result;    
            }
        }

        public static LinqIt.Components.GridLayout WidePage3Columns
        {
            get { return GetWidePageLayout(3); }
        }

        public static LinqIt.Components.GridLayout WidePage2Columns
        {
            get { return GetWidePageLayout(2); }
        }

        public static LinqIt.Components.GridLayout WidePage1Column
        {
            get { return GetWidePageLayout(1); }
        }

        private static LinqIt.Components.GridLayout GetWidePageLayout(int columns)
        {
            var result = new GridLayout(12);
            result.AddRow().AddCell(12, "Top", "Top", GridLayoutCellType.Placeholder);
            var contentRow = result.AddRow();
            for (int i = 1; i <= columns; i++)
            {
                contentRow.AddCell(12 / columns, "Col" + i, "Column " + i, GridLayoutCellType.Placeholder);
            }
            result.AddRow().AddCell(12, "Bottom", "Bottom", GridLayoutCellType.Placeholder);
            return result;
        }
    }
}