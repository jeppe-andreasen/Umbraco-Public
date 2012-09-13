using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Utils.Web;

namespace UmbracoPublic.Logic.Controllers.Paging
{
    public class PagerCenteredPagingController : PagingController
    {
        public override void Render(HtmlWriter writer, int pageNumber, int firstPage, int lastPage, int pages, bool showEnds, bool renderOuterTag)
        {
            throw new NotImplementedException();
        }
    }
}
