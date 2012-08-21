﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;

namespace UmbracoPublic.Logic.Entities
{
    public class NewsListModule : GridModule
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

        public bool DisablePaging
        {
            get { return GetValue<bool>("disablePaging"); }
        }

        public Id[] CategorizationIds
        {
             get { return GetValue<IdList>("categorizations").ToArray(); }   
        }
    }
}
