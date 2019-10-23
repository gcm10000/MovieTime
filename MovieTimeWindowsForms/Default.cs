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
using System.Threading;

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
            new Thread(() =>
            {
                this.Invoke(new MethodInvoker(() =>
                {
                    listBoxSearch.Items.Clear();

                }));
                var search = txtSearch.Text;
                var result = GetResult("http://movietime-001-site1.itempurl.com/get/search?query=" + search);
                watches = JsonConvert.DeserializeObject<List<SearchWatch>>(result);

                this.Invoke(new MethodInvoker(() =>
                {
                    foreach (var item in watches)
                    {
                        listBoxSearch.Items.Add(item.Title);
                    }
                }));
            })
            { IsBackground = true}.Start();

        }
        private string GetResult(string url)
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
                //using (var responseAsync = await request.GetResponseAsync())
                //{
                //    HttpWebResponse response = (HttpWebResponse) responseAsync;
                //    Stream dataStream = response.GetResponseStream();
                //    StreamReader reader = new StreamReader(dataStream);
                //    result = await reader.ReadToEndAsync();

                //    reader.Close();
                //    dataStream.Close();
                //}

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
        private void ListBoxSearch_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.listBoxSearch.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                new Thread(() =>
                {
                    var id = watches.Find(x => x.Title == listBoxSearch.Items[index].ToString()).ID;
                    var result = GetResult("http://movietime-001-site1.itempurl.com/get/information?id=" + id.ToString());
                    var watch = JsonConvert.DeserializeObject<Watch>(result);
                    this.Invoke(new MethodInvoker(() =>
                    {
                        Player player = new Player(watch);
                        player.ShowDialog();
                    }));
                })
                { IsBackground = true}.Start();
            }
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnSearch_Click(this, new EventArgs());
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
    }
}
