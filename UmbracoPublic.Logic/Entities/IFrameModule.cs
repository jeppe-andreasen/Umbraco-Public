using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;

namespace UmbracoPublic.Logic.Entities
{
    public class IFrameModule : GridModule
    {
        public string Url
        {
            get { return this["url"]; }
        }

        public int? Height
        {
            get { return GetValue<int?>("height"); }
        }

        public bool ShowBorder
        {
            get { return GetValue<bool>("showBorder"); }
        }

        public bool ShowScrollbars
        {
            get { return GetValue<bool>("showScrollbars"); }
        }
    }
}
