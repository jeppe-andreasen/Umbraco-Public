using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Cms;
using LinqIt.Components;
using LinqIt.Utils.Web;

namespace UmbracoPublic.WebSite.masterpages
{
    public partial class SiteSearchResultPage : System.Web.UI.MasterPage, IGridModuleControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            litOutput.Text = HtmlWriter.Generate(GenerateOutput);
        }

        private static void GenerateOutput(HtmlWriter writer)
        {
            var page = CmsService.Instance.GetItem<UmbracoPublic.Logic.Entities.SiteSearchResultPage>();
            if (!string.IsNullOrEmpty(page.Headline))
                writer.RenderFullTag(HtmlTextWriterTag.H1, page.Headline);
        }

        public GridLayout GetGridLayout()
        {
            var result = new GridLayout(12);
            result.AddRow().AddCell(12, "Top", "Top", GridLayoutCellType.Placeholder);
            var contentRow = result.AddRow();
            contentRow.AddCell(9, "Main", "Main", GridLayoutCellType.Placeholder);
            contentRow.AddCell(3, "Right", "Right", GridLayoutCellType.Placeholder);
            result.AddRow().AddCell(12, "Bottom", "Bottom", GridLayoutCellType.Placeholder);
            return result;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            sectionA.Visible = !plhTop.IsEmpty;
        }
    }
}