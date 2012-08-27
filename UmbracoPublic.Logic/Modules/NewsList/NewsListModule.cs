using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;

namespace UmbracoPublic.Logic.Modules.NewsList
{
    public class NewsListModule : BaseModule
    {
        public string Headline
        {
            get { return GetValue<string>("headline"); }
        }

        public Text Intro
        {
            get { return GetValue<Text>("intro"); }
        }

        public int ItemsPerPage
        {
            get { return GetValue<int?>("itemsPerPage") ?? 10; }
        }

        public Id[] CategorizationIds
        {
             get { return GetValue<IdList>("categorizations").ToArray(); }   
        }

        public int? MaxItemsShown
        {
            get { return GetValue<int?>("maxItemsShown"); }
        }

        public bool ShowPager
        {
            get
            {
                return GetValue<bool>("showPager");
            }
        }

        public string NewsPageLinkText
        {
            get { return GetValue<string>("newsPageLinkText"); }
        }
    }
}
