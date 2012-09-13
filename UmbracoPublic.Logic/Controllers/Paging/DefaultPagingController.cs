using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using LinqIt.Utils.Web;

namespace UmbracoPublic.Logic.Controllers.Paging
{
    public class DefaultPagingController : PagingController
    {
        public override void Render(HtmlWriter writer, int pageNumber, int firstPage, int lastPage, int pages, bool showEnds, bool renderOuterTag)
        {
            if (renderOuterTag)
                writer.RenderBeginTag(HtmlTextWriterTag.Div, "pagination");
            writer.RenderBeginTag(HtmlTextWriterTag.Ul);

            if (pageNumber > 1)
                RenderPageLink(writer, pageNumber - 1, "Prev", "prev", true);
            else
                RenderPageLink(writer, null, "Prev", "prev disabled", true);

            for (var i = firstPage; i <= lastPage; i++)
                RenderPageLink(writer, i == pageNumber ? (int?)null : i, i.ToString(), i == pageNumber ? "page disabled" : "page", true);

            if (pageNumber < pages)
                RenderPageLink(writer, pageNumber + 1, "Next", "next", true);
            else
                RenderPageLink(writer, null, "Next", "next disabled", true);

            writer.RenderEndTag(); // ul
            if (renderOuterTag)
                writer.RenderEndTag(); // div.pagination
        }
    }
}
