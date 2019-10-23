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

        //GET ID
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.AddHeader("Content-Type", "application/json");
            try
            {
                if (int.TryParse(Request.QueryString["id"], out int id))
                {
                    var ConnectionString = System.Configuration.ConfigurationManager.
    ConnectionStrings["connection"].ConnectionString;
                    Commands cmd = new Commands(ConnectionString);
                    LibraryShared.Watch watch = cmd.GetWatch(id);
                    string json = JsonConvert.SerializeObject(watch, Formatting.Indented);
                    Response.Write(json);

                }
                else
                {
                    Response.StatusCode = 422;
                    string json = JsonConvert.SerializeObject("Parse int error.");
                    Response.Write(json);

                }
            }
            catch (ArgumentNullException)
            {
                Response.StatusCode = 422;
                string json = JsonConvert.SerializeObject("Does not exists record with ID requested.");
                Response.Write(json);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 422;
                string json = JsonConvert.SerializeObject($"Error: {ex.Message}.");
                Response.Write(json);
            }


        }
    }
}