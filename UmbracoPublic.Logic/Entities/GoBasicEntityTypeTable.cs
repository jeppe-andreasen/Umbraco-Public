using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqIt.Cms.Data;
using UmbracoPublic.Logic.Modules.Forms;

namespace UmbracoPublic.Logic.Entities
{
    public class GoBasicEntityTypeTable : EntityTypeTable
    {
        public GoBasicEntityTypeTable()
        {
            this.AddType("/FormsFieldFolder", typeof(FormsFieldFolder));
            this.AddType("/FormsActionFolder", typeof(FormsActionFolder));
            this.AddType("/FormsField/FormsTextBoxField", typeof(FormsTextBoxField));
            this.AddType("/FormsField/FormsDropDownField", typeof(FormsDropDownField));
            this.AddType("/FormsField/FormsCheckBoxField", typeof(FormsCheckBoxField));
            this.AddType("/FormsField/FormsFileUploadField", typeof(FormsFileUploadField));
            this.AddType("/FormsField/FormsEmailField", typeof(FormsEmailField));
        }
    }
}
