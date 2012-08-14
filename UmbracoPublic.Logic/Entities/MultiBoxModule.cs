using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;

namespace UmbracoPublic.Logic.Entities
{
    public class MultiBoxModule : GridModule
    {
        public string Headline
        {
            get { return GetValue<string>("headline"); }
        }

        public Text Intro
        {
            get { return GetValue<Text>("intro"); }
        }

        public Image Image
        {
            get { return GetValue<Image>("image"); }
        }

        public Html Body
        {
            get { return GetValue<Html>("body"); }
        }

        public Link Link
        {
            get 
            { 
                return GetValue<LinkList>("link").FirstOrDefault();
            }
        }
    }
}
