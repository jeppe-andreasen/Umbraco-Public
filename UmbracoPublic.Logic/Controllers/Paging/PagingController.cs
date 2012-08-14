using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using LinqIt.Utils.Web;
using UmbracoPublic.Logic.Utilities;

namespace UmbracoPublic.Logic.Controllers.Paging
{
    public abstract class PagingController
    {
        public abstract void Render(LinqIt.Utils.Web.HtmlWriter writer, int pageNumber, int firstPage, int lastPage, int pages, bool showEnds);

        protected void RenderPageLink(HtmlWriter writer, int? pageLink, string text, string cssClass, bool addSpan)
        {
            writer.AddClass(cssClass);
            writer.RenderBeginTag(HtmlTextWriterTag.Li);
            if (pageLink.HasValue)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Title, text);
                writer.RenderBeginLink(Urls.ReplaceUrlParameter(QueryStringKey.PageNumber, pageLink.Value.ToString()));
            }
            else
                writer.RenderBeginLink("#");

            if (addSpan)
                writer.RenderBeginTag(HtmlTextWriterTag.Span);
            writer.Write(text);
            if (addSpan)
                writer.RenderEndTag();

            writer.RenderEndTag(); // a or strong
            writer.RenderEndTag(); // li
        }
    }
}
