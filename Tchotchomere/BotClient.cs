using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Tchotchomere
{
    class BotClient
    {
        private Uri Host;
        private List<string> NewUrls = new List<string>();
        private List<string> OldUrls = new List<string>();
        private bool CheckAllWebSite = false;

        public delegate void Result(EventResult Result);
        public event Result ResultEvent;

        public BotClient(string baseUrl, bool checkAllWebSite)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            // Use SecurityProtocolType.Ssl3 if needed for compatibility reasons

            var host = new Uri(baseUrl);
            this.Host = new Uri(host.Scheme + "://" + host.Host);
            this.CheckAllWebSite = checkAllWebSite;
        }
        public void Start()
        {
            if (CheckAllWebSite)
                AccessingWithCheck(Host);
            else
                AccessingPageWithoutCheck(Host);
        }

        private void AccessingWithCheck(Uri address)
        {
            new Task(() =>
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.Encoding = System.Text.Encoding.UTF8;
                    webClient.DownloadStringCompleted +=
                    (sender, e) =>
                    {
                        if (e.Error == null)
                        {
                            if ((NewUrls.Count == 0) || ((NewUrls.Contains(address.ToString())) && (!OldUrls.Contains(address.ToString()))))
                                OldUrls.Add(address.ToString());
                            List<string> urls = LinkExtractor.ExtractUrl(e.Result, address.ToString());
                            foreach (var url in urls.ToList())
                            {
                                string strUrl = url;
                                if (!strUrl.EndsWith("/"))
                                {
                                    if (!Path.HasExtension(new Uri(strUrl).AbsolutePath))
                                        strUrl = url + "/";
                                }
                                if ((!NewUrls.Contains(strUrl)) && (!OldUrls.Contains(strUrl)))
                                {
                                    NewUrls.Add(strUrl);
                                    //Console.WriteLine(url);
                                }
                            }
                            NewUrls.Remove(NewUrls[0]);
                            ResultEvent.Invoke(new EventResult(address.ToString(), NewUrls, OldUrls));
                            if (NewUrls.Count > 0)
                                AccessingWithCheck(new Uri(NewUrls[0]));
                        }
                        else
                        {
                            ResultEvent.Invoke(new EventResult(e.Error));
                        }
                    };
                    webClient.DownloadStringAsync(address);
                }
            }).Start();
        }
        private void AccessingPageWithoutCheck(Uri address)
        {
            new Task(()=> 
            {
                using (WebClient webClient = new WebClient())
                {
                    webClient.Encoding = System.Text.Encoding.UTF8;
                    webClient.DownloadStringCompleted += 
                    (sender, e) => 
                    {
                        if (e.Error == null)
                        {
                            OldUrls.Add(address.ToString());
                            List<string> urls = LinkExtractor.ExtractUrl(e.Result, address.ToString());
                            foreach (var url in urls.ToList())
                            {
                                string strUrl = url;
                                if (!strUrl.EndsWith("/"))
                                {
                                    if (!Path.HasExtension(new Uri(strUrl).AbsolutePath))
                                        strUrl = url + "/";
                                }
                                if (!NewUrls.Contains(strUrl) && (!OldUrls.Contains(strUrl)))
                                    NewUrls.Add(strUrl);
                                Console.WriteLine(strUrl);
                            }
                        }
                    };
                    webClient.DownloadStringAsync(address);
                }
            }).Start();
        }
    }
    class EventResult : EventArgs
    {
        public string ResultUrl { get; set; }
        public ICollection<string> NewUrls { get; set; }
        public ICollection<string> OldUrls { get; set; }
        public Exception Exception { get; set; }
        public EventResult(string ResultUrl, ICollection<string> NewUrls, ICollection<string> OldUrls)
        {
            this.ResultUrl = ResultUrl;
            this.NewUrls = NewUrls;
            this.OldUrls = OldUrls;
        }
        public EventResult(Exception Exception)
        {
            this.Exception = Exception;
        }
    }
}
