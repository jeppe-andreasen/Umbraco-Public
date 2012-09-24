using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UmbracoPublic.Logic.Modules.Forms
{
    public class FormsModule : BaseModule
    {
        public IEnumerable<FormsField> Fields
        {
            get { return GetChildrenOfType<FormsFieldFolder>().SelectMany(f => f.Fields); }
        }

        public IEnumerable<FormsAction> Actions
        {
            get { return GetChildrenOfType<FormsActionFolder>().SelectMany(f => f.Actions); }
        }

        public string Headline { get { return GetValue<string>("headline"); } }
    }
}
