using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UmbracoPublic.WebSite.test
{
    public partial class TestDatabase : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.AppSettings["umbracoDbDSN"]);
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT * FROM umbracoUser", connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    litOutput.Text += reader.GetString(6) + ",";
                }
                reader.Close();
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }
    }
}