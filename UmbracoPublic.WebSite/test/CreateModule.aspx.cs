using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using UmbracoPublic.Logic.Utilities;
using umbraco.cms.businesslogic.datatype;

namespace UmbracoPublic.WebSite.test
{
    public partial class CreateModule : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                foreach (var def in DataTypeDefinition.GetAll())
                    ddlParameterType.Items.Add(new ListItem(def.Text, def.Id.ToString()));   
            }
            
        }

        protected void OnCreateClicked(object sender, EventArgs e)
        {
            ModuleHelper.CreateModule(txtName.Text, Parameters);
        }

        protected void OnAddParameterClicked(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(ddlParameterType.SelectedValue);
            Parameters.Add(txtParameterName.Text, id);

            repeater.DataSource = Parameters;
            repeater.DataBind();
        }

        protected Dictionary<string, int> Parameters
        {
            get 
            { 
                object result = ViewState["Parameters"];
                if (result == null)
                {
                    result = new Dictionary<string, int>();
                    ViewState["Parameters"] = result;
                }
                return (Dictionary<string, int>) result;
            }
        }
    }
}