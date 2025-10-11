using SoACA1v2.DataModels;
using System.Net.Http.Json;

namespace SoACA1v2.Services
{
    public class TicketMasterClient
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;

        public TicketMasterClient(HttpClient http, IConfiguration config)
        {
            _http = http;
            _apiKey = config["Keys:TicketMasterAPI"] ?? string.Empty;
        }

        public async Task<RootObject?> GetEventsByCity(string city)
        {
            var url = $"events.json?apikey={_apiKey}&city={city}";

            try
            {
                return await _http.GetFromJsonAsync<RootObject>(url);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP error calling Ticketmaster API: {ex.Message}");
                return null;
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine($"Bad response format: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                return null;
            }
        }
    }
}