using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryShared;

namespace MovieTimeWindowsForms
{
    public partial class Default : Form
    {
        List<SearchWatch> watches;
        public Default()
        {
            InitializeComponent();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            listBoxSearch.Items.Clear();
            var search = txtSearch.Text;
            var result = GetResult("http://localhost:54691/get/search?query=" + search);
            watches = JsonConvert.DeserializeObject<List<SearchWatch>>(result);

            foreach (var item in watches)
            {
                listBoxSearch.Items.Add(item.Title);
            }

        }
        public static string GetResult(string url)
        {
            string result = string.Empty;

            using (var clientAPI = new HttpClient())
            {
                ServicePointManager.Expect100Continue = true;
                //SecurityProtocolType.Tls12 missing on Framework 4.0 only
                //SecurityProtocolType.Tls12  == (SecurityProtocolType)(0xc00)
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xc00);

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.KeepAlive = false;
                request.UserAgent = @"Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36";
                request.Method = "GET";
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    result = reader.ReadToEnd();

                    reader.Close();
                    dataStream.Close();
                }
            }

            return result;
        }

        private void ListBoxSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxSearch.Items.Count > 0)
            {
                var index = listBoxSearch.SelectedIndex;
                var id = watches.Find(x => x.Title == listBoxSearch.Items[index].ToString()).ID;
                var result = GetResult("http://localhost:54691/get/information?id=" + id.ToString());
                var watch = JsonConvert.DeserializeObject<Watch>(result);
                Player player = new Player(watch);
                player.ShowDialog();

            }
        }
    }
}
