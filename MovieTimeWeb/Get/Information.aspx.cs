using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using LibraryShared;


namespace MovieTimeWeb.Get
{
    public partial class Information : System.Web.UI.Page
    {
        const string ConnectionString = @"Data Source=GABRIEL-PC\SQLEXPRESS;Initial Catalog=movietime_database;Integrated Security=True";

        //GET ID
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.AddHeader("Content-Type", "application/json");
            try
            {
                if (int.TryParse(Request.QueryString["id"], out int id))
                {
                    Commands cmd = new Commands(ConnectionString);
                    LibraryShared.Watch watch = cmd.GetWatch(id);
                    string json = JsonConvert.SerializeObject(watch);
                    Response.Write(json);

                }
                else
                {
                    Response.StatusCode = 422;
                    string json = JsonConvert.SerializeObject("Parse int error.");
                    Response.Write(json);

                }
                //var js = new JavaScriptSerializer();
                //var arraystr = new string[] { "A", "B", "C" };
                //string myJson = js.Serialize(arraystr);,
            }
            catch (ArgumentNullException)
            {
                Response.StatusCode = 422;
                string json = JsonConvert.SerializeObject("Does not exists record with ID requested.");
                Response.Write(json);
            }


        }
    }
}