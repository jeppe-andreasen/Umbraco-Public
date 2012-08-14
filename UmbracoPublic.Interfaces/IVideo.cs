using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Utils.Web;

namespace UmbracoPublic.Interfaces
{
    public interface IVideo
    {
        string Id { get; }
        string Title { get; }
        IVideoProvider Provider { get; }
        void Render(HtmlWriter writer);
    }
}
