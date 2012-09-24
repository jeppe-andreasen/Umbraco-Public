using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;

namespace UmbracoPublic.Logic.Modules.Forms
{
    public class FormsAction : Entity
    {
        public override string TemplatePath
        {
            get { return "/FormsAction"; }
        }

        internal virtual void Execute(List<FieldSpecification> specifications)
        {
        }
    }
}
