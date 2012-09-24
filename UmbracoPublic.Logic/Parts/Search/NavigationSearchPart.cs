using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.UI;
using LinqIt.Ajax.Parsing;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.Logic.Parts.Search
{
    public class NavigationSearchPart : BasePart
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            try
            {
                var searchResultUrl = Urls.GetSystemUrl(SystemKey.SiteSearchResultPage);
                ModuleScripts.RegisterInitScript("search", new JSONString(searchResultUrl));
            }
            catch (ConfigurationErrorsException)
            {
                Visible = false;
            }
        }

        protected override void RenderPart(LinqIt.Utils.Web.HtmlWriter writer)
        {
            if (Visible)
            {
                writer.AddAttribute("action", "Søg");
                writer.RenderBeginTag(HtmlTextWriterTag.Div, "navbar-search pull-right");
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "text");
                writer.AddAttribute("placeholder", "Søg");
                writer.RenderFullTag(HtmlTextWriterTag.Input, "", "search-query");
                writer.AddAttribute(HtmlTextWriterAttribute.Type, "submit");
                writer.RenderBeginTag(HtmlTextWriterTag.Button, "btn");
                writer.RenderFullTag(HtmlTextWriterTag.Span, "Submit");
                writer.RenderEndTag(); // button.btn
                writer.RenderEndTag(); // div.navbar-search pull-right
            }
        }
    }
}
