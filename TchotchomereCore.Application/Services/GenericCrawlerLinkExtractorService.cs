using Microsoft.Extensions.Logging;
using TchotchomereCore.Domain.Entities;
using TchotchomereCore.Domain.Interfaces;
using TchotchomereCore.Infrastructure;
using TchotchomereCore.Infrastructure.Helpers;
using TchotchomereCore.Infrastructure.Sql.Repositories;

namespace TchotchomereCore.Application.Services;

public class GenericCrawlerLinkExtractorService : ICrawlerLinkExtractor
{
    private readonly IExtractedURLRepository _extractURLRepository;
    private readonly IInitialURLRepository _initialURLRepository;
    private readonly ILogger<GenericCrawlerLinkExtractorService> _logger;

    //private string _hostAsString => _host.ToString();
    private readonly Guid _trace;

    //private Uri _host = null!;

    //private List<string> NewUrls = new List<string>();
    //private List<string> OldUrls = new List<string>();

    public GenericCrawlerLinkExtractorService(
        IExtractedURLRepository urlExtractRepository,
        ILogger<GenericCrawlerLinkExtractorService> logger,
        IInitialURLRepository initialURLRepository)
    {
        _extractURLRepository = urlExtractRepository;
        _logger = logger;
        _trace = Guid.NewGuid();
        _initialURLRepository = initialURLRepository;
    }

    public async Task StartAsync(CancellationToken stoppingToken = default)
    {
        //var g = new InitialURL("https://teutorrent.theproxy.ws/");
        //await _initialURLRepository.AddAsync(g, stoppingToken);

        var initialURLs = await _initialURLRepository.GetAllAsync(stoppingToken);

        foreach (var initialURL in initialURLs)
        {
            var extractedUrlFromInitialUrl = new ExtractedURL(initialURL.Uri, _trace, null!);

            await VisitWebPageAndExtractURLsAsync(extractedUrlFromInitialUrl);

            await _initialURLRepository
                .UpdateProcessedDateTimeAsCurrentDateTimeAsync(initialURL.Id, stoppingToken);
        }

        await StartExtractorAsync(stoppingToken);
    }

    private async Task StartExtractorAsync(CancellationToken stoppingToken = default)
    {
        //var newURL = new ExtractedURL(uri, _trace);

        //await _extractURLRepository.AddIfUrlIsNotRegistedAsync(newURL, stoppingToken);

        while (await _extractURLRepository.AnyExtractedURLNotProcessedAsync())
        {
            await AccessMappedWebPagesAsync();
        }
    }

    private async Task AccessMappedWebPagesAsync()
    {
        var extractedURLNotProcessed = await _extractURLRepository
            .GetExtractedURLNotProcessedAsync(quantity: 200);

        foreach (var extractedURL in extractedURLNotProcessed)
        {
            _logger.LogInformation("Current BaseUrl: {BaseUrl} | Url: {Url}", 
                extractedURL.BaseUrl, extractedURL.Url);
            
            await VisitWebPageAndExtractURLsAsync(extractedURL);
        }
    }

    private async Task VisitWebPageAndExtractURLsAsync(ExtractedURL currentExtractedURL)
    {
        var content = await GetRawPageAsync(currentExtractedURL.Url);

        if (content is null)
            return;
        
        await _extractURLRepository
            .UpdateProcessedDateTimeAsCurrentDateTimeAsync(currentExtractedURL.Id);

        var host = new Uri(currentExtractedURL.Uri.Scheme + "://" + currentExtractedURL.Uri.Host);

        var extractedURLs = GetUris(content, host)
            .Where(uri => uri.Host == currentExtractedURL.Uri.Host)
            .Select(uri => new ExtractedURL(uri, _trace, content))
            .ToList();

        var count = await _extractURLRepository.AddRangeIfUrlsAreNotRegistedAsync(extractedURLs);
        _logger.LogInformation("It was founded {Count} records.", extractedURLs.Count);
        _logger.LogInformation("They have saved {count} records successfully.", count);
    }

    //private static async Task<string> GetRawPageAsyncC(string address)
    //{
    //    using var httpClient = new HttpClient();
    //    // define the retry policy with delay in Polly
    //    var policyWithDelay = Policy
    //        .HandleResult<HttpResponseMessage>(r => r.StatusCode == HttpStatusCode.TooManyRequests) // retry on 429 errors
    //        .WaitAndRetryAsync(
    //          7,// up to 7 attempts
    //          retryAttempt => TimeSpan.FromSeconds(5) // 5-second delay after each attempt
    //        );

    //    // make the request with the specified retry policy
    //    var response = await policyWithDelay.ExecuteAsync(() => httpClient.GetAsync("https://scrapeme.live/shop/"));
    //}

    private static async Task<string?> GetRawPageAsync(string address)
    {
        using var httpClient = new HttpClient();
        var response = await httpClient.RequestWithBackoffRetry(address);

        if (response is null)
            return null;

        if (!response.IsSuccessStatusCode)
            return null;

        var content = await response.Content.ReadAsStringAsync();
        return content;
    }



    private static List<Uri> GetUris(string rawHtml, Uri host)
    {
        //if (!OldUrls.Contains(address))
        //    OldUrls.Add(address);

        var allURLsFromWebPage = LinkExtractor.ExtractUrl(rawHtml);

        var URLsWithinWebsite = allURLsFromWebPage.Where(x => host.IsBaseOf(new Uri(x)))
            .Select(GetFormattedURL)
            .Select(x => new Uri(x))
            .ToList();

        return URLsWithinWebsite;

        //foreach (var url in URLsWithinWebsite)
        //{
        //    var strUrl = GetFormattedURL(url);

        //    if ((!NewUrls.Contains(strUrl)) && (!OldUrls.Contains(strUrl)))
        //    {
        //        NewUrls.Add(strUrl);
        //    }
        //}
        //NewUrls.Remove(NewUrls[0]);
    }

    private static string GetFormattedURL(string url)
    {
        if (!url.EndsWith('/'))
        {
            if (!Path.HasExtension(new Uri(url).AbsolutePath))
                return url + "/";
        }

        return url;
    }
}