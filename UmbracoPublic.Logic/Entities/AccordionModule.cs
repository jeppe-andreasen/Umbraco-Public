using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;
using LinqIt.Components.Data;

namespace UmbracoPublic.Logic.Entities
{
    public class AccordionModule : GridModule
    {
        //public string Headline
        //{
        //    get { return GetValue<string>("Headline"); }
        //}

        //public Text Content
        //{
        //    get { return GetValue<Text>("Content"); }
        //}

        public AccordionData Data
        {
            get { return AccordionData.Parse(this.Id.ToString(), string.Empty, string.Empty, this["accordionContent"]); }
        }


    }
}
