using Microsoft.AspNetCore.Mvc;

namespace apigw_monitor.Controllers
{
    public class BackendThrottler
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly SemaphoreSlim _semaphore;

        public BackendThrottler(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _semaphore = new SemaphoreSlim(30); // Limit 30 concurrent connections
        }

        public async Task<IActionResult> ForwardRequest(HttpContext context)
        {
            if (!await _semaphore.WaitAsync(TimeSpan.FromSeconds(5)))
            {
                return new StatusCodeResult(429); // Too Many Requests
            }

            try
            {
                var client = _httpClientFactory.CreateClient("Butch");

                var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"https://apigw-monitor.8elements.mobi"); // Adjust endpoint if needed
                requestMessage.Content = new StreamContent(context.Request.Body);

                foreach (var header in context.Request.Headers)
                {
                    requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
                }

                var response = await client.SendAsync(requestMessage);
                var content = await response.Content.ReadAsStringAsync();

                var mediaType = response.Content.Headers.ContentType?.MediaType ?? "application/json";
                return new ContentResult
                {
                    Content = content,
                    StatusCode = (int)response.StatusCode,
                    ContentType = mediaType
                };
            }
            catch (Exception ex)
            {
                return new ObjectResult(new { error = ex.Message }) { StatusCode = 500 };
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
