using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LibraryShared;

namespace MovieTimeWeb.Get
{
    public partial class Search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.AddHeader("Content-Type", "application/json");
            try
            {
                var query = Request.QueryString["query"];
                Commands cmd = new Commands(Statics.ConnectionString);
                var search = cmd.SearchWatch(query);
                string json = JsonConvert.SerializeObject(search);
                Response.StatusCode = 200;
                Response.Write(json);

            }
            catch (Exception ex)
            {
                Response.StatusCode = 422;
                string json = JsonConvert.SerializeObject(ex.Message);
                Response.Write(json);
            }
        }
    }
}