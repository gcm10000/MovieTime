using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TchotchomereCore
{
    class BotClient
    {
        public Uri Host { private set; get; }
        private List<string> NewUrls = new List<string>();
        private List<string> OldUrls = new List<string>();
        private bool CheckAllWebSite = false;

        public delegate void Result(ResultEventArgs Result);
        public event Result ResultEvent;

        public BotClient(string baseUrl, bool checkAllWebSite)
        {
            //ServicePointManager.Expect100Continue = true;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //(SecurityProtocolType)3072;
            // Use SecurityProtocolType.Ssl3 if needed for compatibility reasons

            //NEVER_EAT_POISON_Disable_CertificateValidation();

            var host = new Uri(baseUrl);
            this.Host = new Uri(host.Scheme + "://" + host.Host);
            this.CheckAllWebSite = checkAllWebSite;
        }

        public void Start()
        {
            NewUrls.Add(Host.ToString());
            if (CheckAllWebSite)
                AccessingWithCheckV2(Host).Wait();
            else
            {

            }
                //AccessingPageWithoutCheck(Host);
        }

        private async Task AccessingWithCheckV2(Uri address)
        {
            try
            {
                using var httpClient = new HttpClient();

                var response = await httpClient.GetAsync(address);

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                ExtrairURLs(address, content);
            }
            catch (Exception ex)
            {
                ResultEvent.Invoke(new ResultEventArgs(ex));
            }
            finally
            {
                if (NewUrls.Count > 0)
                    await AccessingWithCheckV2(new Uri(NewUrls[0]));
            }
        }

        //private void AccessingWithCheck(Uri address)
        //{
        //    new Task(() =>
        //    {
        //        using (WebClient webClient = new WebClient())
        //        {
        //            webClient.Encoding = System.Text.Encoding.UTF8;
        //            var result = string.Empty;
        //            webClient.DownloadStringCompleted +=
        //            (sender, e) =>
        //            {
        //                if (e.Error == null)
        //                {
        //                    result = e.Result;
        //                    ExtrairMagnetDeTorrent(address, result);
        //                    if (NewUrls.Count > 0)
        //                        AccessingWithCheck(new Uri(NewUrls[0]));
        //                }
        //                else
        //                {
        //                    ResultEvent.Invoke(new ResultEventArgs(e.Error));
        //                }
        //            };
        //            webClient.DownloadStringAsync(address);
        //        }
        //    }).Start();
        //}

        private void ExtrairURLs(Uri address, string result)
        {
            if ((NewUrls.Count == 0) || ((NewUrls.Contains(address.ToString())) && (!OldUrls.Contains(address.ToString()))))
                OldUrls.Add(address.ToString());

            List<string> urls = LinkExtractor.ExtractUrl(result);

            var URLsWithinWebsite = urls.Where(x => Host.IsBaseOf(new Uri(x)))
                .ToList();

            foreach (var url in URLsWithinWebsite)
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
                }
            }
            NewUrls.Remove(NewUrls[0]);

            if (ResultEvent != null)
                ResultEvent.Invoke(new ResultEventArgs(address.ToString(), result, urls, NewUrls, OldUrls));
        }

        //private void AccessingPageWithoutCheck(Uri address)
        //{
        //    new Task(()=> 
        //    {
        //        using (WebClient webClient = new WebClient())
        //        {
        //            webClient.Encoding = System.Text.Encoding.UTF8;
        //            webClient.DownloadStringCompleted += 
        //            (sender, e) => 
        //            {
        //                if (e.Error == null)
        //                {
        //                    OldUrls.Add(address.ToString());
        //                    List<string> urls = LinkExtractor.ExtractUrlSameHost(e.Result, address.ToString());
        //                    foreach (var url in urls.ToList())
        //                    {
        //                        string strUrl = url;
        //                        if (!strUrl.EndsWith("/"))
        //                        {
        //                            if (!Path.HasExtension(new Uri(strUrl).AbsolutePath))
        //                                strUrl = url + "/";
        //                        }
        //                        if (!NewUrls.Contains(strUrl) && (!OldUrls.Contains(strUrl)))
        //                            NewUrls.Add(strUrl);
        //                        Console.WriteLine(strUrl);
        //                    }
        //                }
        //            };
        //            webClient.DownloadStringAsync(address);
        //        }
        //    }).Start();
        //}
    }

    class ResultEventArgs : EventArgs
    {
        public string Address { get; set; }
        public ICollection<string> NewUrls { get; set; }
        public ICollection<string> TotalLinks { get; set; }
        public ICollection<string> OldUrls { get; set; }
        public string ResultHtml { get; set; }
        public Exception Exception { get; set; }
        public ResultEventArgs(
            string Address, 
            string ResultHtml, 
            ICollection<string> TotalLinks, 
            ICollection<string> NewUrls, 
            ICollection<string> OldUrls)
        {
            this.Address = Address;
            this.ResultHtml = ResultHtml;
            this.TotalLinks = TotalLinks;
            this.NewUrls = NewUrls;
            this.OldUrls = OldUrls;
        }
        public ResultEventArgs(Exception Exception)
        {
            this.Exception = Exception;
        }
    }
}
