using SoACA1v2.DataModels;
using System.Net.Http.Json;
using SoACA1v2.Services.HTTP.Interfaces;
using SoACA1v2.Services.Interfaces;

namespace SoACA1v2.Services
{
    public class TicketMasterClient : HttpClientBase , ITicketMasterClient
    {
        public TicketMasterClient(HttpClient http, IConfiguration config)
            : base(http, config, "TicketMasterAPI") { }
        
        public async Task<RootObject?> GetEvents(string? countryCode, string? genreId, DateOnly?  startDate, DateOnly? endDate, string? keyword = null)
        {
            var queryParams = new List<string>
            {
                $"apikey={_apiKey}&classificationName=Music",
                $"size=10"
            };

            if (!string.IsNullOrWhiteSpace(countryCode))
                queryParams.Add($"countryCode={countryCode}");

            if (string.IsNullOrWhiteSpace(genreId) && string.IsNullOrWhiteSpace(keyword) && string.IsNullOrWhiteSpace(countryCode))
            {
                queryParams.Add($"countryCode=IE");
            }

            if (!string.IsNullOrWhiteSpace(genreId))
                queryParams.Add($"genreId={genreId}");

            if (startDate.HasValue)
                queryParams.Add($"startDateTime={startDate.Value:yyyy-MM-dd}T00:00:00Z");

            if (endDate.HasValue)
                queryParams.Add($"endDateTime={endDate.Value:yyyy-MM-dd}T23:59:59Z");
            
            if (!string.IsNullOrWhiteSpace(keyword))
                queryParams.Add($"keyword={Uri.EscapeDataString(keyword)}");

            var url = $"events.json?{string.Join("&", queryParams)}";

            return await GetAsync<RootObject>(url);
        }
    }
}