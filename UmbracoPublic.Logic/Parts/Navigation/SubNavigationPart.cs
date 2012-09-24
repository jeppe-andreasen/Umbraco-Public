using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using LinqIt.Cms;
using UmbracoPublic.Logic.Entities;
using UmbracoPublic.Logic.Services;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.Logic.Parts.Navigation
{
    public class SubNavigationPart : BasePart
    {
        private IEnumerable<MenuItem> _menuItems;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            var isNewsListMonthPage = false;

            //test if this is a month page in the news archive a hide accordingly.
            var page = CmsService.Instance.GetItem<LinqIt.Cms.Data.Page>();
            var newsArchivePath = Paths.GetSystemPath(SystemKey.NewsArchivePage);
            if (!string.IsNullOrEmpty(newsArchivePath) && page.Path.StartsWith(newsArchivePath))
            {
                if (page.Path.Split('/').Length == newsArchivePath.Split('/').Length + 2)
                    isNewsListMonthPage = true;
            }
            _menuItems = DataService.Instance.GetSubMenuItems(isNewsListMonthPage ? page.GetParent<LinqIt.Cms.Data.Page>() : null);
            Visible = _menuItems.Any();
        }

        protected override void RenderPart(LinqIt.Utils.Web.HtmlWriter writer)
        {
            if (!Visible)
                return;

            writer.RenderBeginTag(HtmlTextWriterTag.Div, "navbar");
            writer.RenderBeginTag(HtmlTextWriterTag.Div, "navbar-inner");
            writer.RenderBeginTag(HtmlTextWriterTag.Ul, "nav");
            foreach (var menuItem in _menuItems)
            {
                if (menuItem.Selected || menuItem.Expanded)
                    writer.AddClass("active");
                writer.RenderBeginTag(HtmlTextWriterTag.Li);
                writer.AddAttribute(HtmlTextWriterAttribute.Href, menuItem.Url);
                writer.RenderFullTag(HtmlTextWriterTag.A, menuItem.DisplayName);
                writer.RenderEndTag(); // li.active
            }
            writer.RenderEndTag(); // ul.nav
            writer.RenderEndTag(); // div.navbar-inner
            writer.RenderEndTag(); // div.navbar
        }
    }
}
