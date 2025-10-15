namespace SoACA1v2.Services;

public abstract class HttpClientBase
{
        protected readonly HttpClient _http;
        protected readonly string _apiKey;

        protected HttpClientBase(HttpClient http, IConfiguration config, string keyName)
        {
            _http = http;
            _apiKey = config[$"Keys:{keyName}"] ?? string.Empty;
        }
        
        protected async Task<T?> GetAsync<T>(string url)
        {
            try
            {
                return await _http.GetFromJsonAsync<T>($"{url}");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP error calling API: {ex.Message}");
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine($"Unsupported response format: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            return default;
        }
}