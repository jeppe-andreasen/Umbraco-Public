using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;
using LinqIt.Components.Data;

namespace UmbracoPublic.Logic.Modules.Accordion
{
    public class AccordionModule : BaseModule
    {
        public AccordionData Data
        {
            get { return AccordionData.Parse(this.Id.ToString(), string.Empty, string.Empty, this["accordionContent"]); }
        }
    }
}
