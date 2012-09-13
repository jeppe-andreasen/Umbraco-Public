using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Cms;
using umbraco;
using umbraco.cms.businesslogic.web;
using UmbracoPublic.Logic.Entities;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.WebSite.test
{
    public partial class FixNewsPaths : System.Web.UI.Page
    {
        protected void Page_Load(object s, EventArgs e)
        {
            var user = global::umbraco.BusinessLogic.User.GetUser(0);
            using (CmsContext.Editing)
            {
                var result = new StringBuilder();
                foreach (var newspage in CmsService.Instance.SelectItems<NewsPage>("/Content//*{NewsPage}"))
                {
                    var sender = new Document(newspage.Id.IntValue);
                    if (newspage.Date.HasValue)
                    {
                        var newsArchivePart = CmsService.Instance.GetSystemPath("NewsArchivePage");
                        var yearPart = newspage.Date.Value.Year.ToString();
                        var monthPart = Urls.MonthArray.Split('|')[newspage.Date.Value.Month - 1];
                        var truePath = Paths.Combine(newsArchivePart, yearPart, monthPart, sender.Text);
                        if (truePath != newspage.Path)
                        {
                            var yearPage = CmsService.Instance.GetItem<NewsListPage>(Paths.Combine(newsArchivePart, yearPart));
                            if (yearPage == null)
                            {
                                var archivePage = CmsService.Instance.GetItem<NewsListPage>(newsArchivePart);
                                yearPage = CmsService.Instance.CreateEntity<NewsListPage>(yearPart, archivePage);
                            }
                            var monthPage = CmsService.Instance.GetItem<NewsListPage>(Paths.Combine(newsArchivePart, yearPart, monthPart));
                            if (monthPage == null)
                                monthPage = CmsService.Instance.CreateEntity<NewsListPage>(monthPart, yearPage);
                            sender.Move(monthPage.Id.IntValue);
                        }
                    }
                    
                }
                library.RefreshContent();
                litOutput.Text = result.ToString();
            }
        }
    }
}



