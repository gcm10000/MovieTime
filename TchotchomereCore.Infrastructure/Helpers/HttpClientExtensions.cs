namespace TchotchomereCore.Infrastructure.Helpers;
public static class HttpClientExtensions
{
    public static async Task<HttpResponseMessage?> RequestWithBackoffRetry(
        this HttpClient client, 
        string url, 
        int maxRetryAttempts = 5, 
        int delay = 3, 
        CancellationToken cancellationToken = default)
    {
        // repeat the request up to maxRetryAttempts times
        for (int attempt = 1; attempt <= maxRetryAttempts; attempt++)
        {
            try
            {
                // try to make the request
                return await client.GetAsync(url, cancellationToken);
            }
            catch (HttpRequestException e)
            {
                if (e.HttpRequestError == HttpRequestError.NameResolutionError)
                    return null;

                // exponential backoff waiting retry logic
                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(delay, attempt)));
            }
        }

        return null;
    }

}
