using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UmbracoPublic.Logic.Controllers.Paging
{
    public class CenteredPagingController : PagingController
    {
        public override void Render(LinqIt.Utils.Web.HtmlWriter writer, int pageNumber, int firstPage, int lastPage, int pages, bool showEnds)
        {
            throw new NotImplementedException();
        }
    }
}
