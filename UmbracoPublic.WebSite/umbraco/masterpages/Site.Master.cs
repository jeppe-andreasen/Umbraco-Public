using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LinqIt.Cms;
using LinqIt.Cms.Data;
using LinqIt.Utils.Collections;
using LinqIt.Utils.Extensions;
using LinqIt.Utils.Web;
using UmbracoPublic.Logic.Entities;
using UmbracoPublic.Logic.Services;
using UmbracoPublic.Logic.Utilities;
using umbraco.cms.businesslogic.web;
using Image = LinqIt.Cms.Data.Image;

namespace UmbracoPublic.WebSite.umbraco.masterpages
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var page = CmsService.Instance.GetItem<Entity>();

            ModuleScripts.RegisterInitScript("navigation");
            ModuleScripts.RegisterInitScript("search");
            ModuleScripts.RegisterInitScript("button");
            ModuleScripts.RegisterInitScript("background");
            
            litDynamicMetaTags.Text = RenderMetaTags(page);

            var theme = Theme.Current;
            if (theme != null)
            {
                litThemeCss.Text = theme.Stylesheets.ToSeparatedString("", "<link href=\"{0}\" rel=\"stylesheet\">");
            }

            var siteRoot = CmsService.Instance.GetItem<SiteRoot>(CmsService.Instance.SitePath);
            var logo = siteRoot.GetValue<Image>("logo");
            litBrand.Text = logo.Exists ? HtmlWriter.Generate(w => w.RenderImageTag(logo.Url, page.EntityName, null)) : page.EntityName;

            var backgroundImage = page.GetValue<Image>("backgroundImage");
            if (!backgroundImage.Exists)
                backgroundImage = siteRoot.DefaultBackgroundImage;
            if (backgroundImage.Exists)
                form1.Attributes.Add("data-bgimage", backgroundImage.Url);
        }

        private static string RenderMetaTags(Entity page)
        {
            var metaTags = new MetaTagCollection();

            #region Description
            var metaDescription = page["metaDescription"];
            if (string.IsNullOrEmpty(metaDescription))
                metaDescription = page["intro"];
            if (string.IsNullOrEmpty(metaDescription))
                metaDescription = page.GetValue<Html>("body").GetExtract(150, true).ToString();
            if (!string.IsNullOrEmpty(metaDescription))
                metaTags.AddName("description", metaDescription);
            #endregion

            #region Keywords
            var keyWords = page["metaKeywords"];
            if (!string.IsNullOrEmpty(keyWords))
                metaTags.AddName("keywords", keyWords);
            #endregion

            #region Author
            var author = page["author"];
            if (string.IsNullOrEmpty(author))
            {
                var document = new Document(page.Id.IntValue);
                var user = new global::umbraco.BusinessLogic.User(document.UserId);
                author = user.Name;    
            }
            if (!string.IsNullOrEmpty(author))
                metaTags.AddName("author", author);

            #endregion

            #region Robots 

            var noIndex = page.GetValue<bool>("noIndex");
            var noFollow = page.GetValue<bool>("noFollow");

            if (noIndex || noFollow)
                metaTags.AddName("ROBOTS", (noIndex? "NOINDEX" : "INDEX") + ", " + (noFollow? "NOFOLLOW" : "FOLLOW"));
            
            #endregion

            return metaTags.ToString();
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            litInitializationScript.Text = ModuleScripts.GetInitializationScript();
        }
    }
}