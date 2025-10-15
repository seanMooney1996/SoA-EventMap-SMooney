using SoACA1v2.DataModels;
using System.Net.Http.Json;

namespace SoACA1v2.Services
{
    public class TicketMasterClient : HttpClientBase
    {
        public TicketMasterClient(HttpClient http, IConfiguration config)
            : base(http, config, "TicketMasterAPI") { }

        public async Task<RootObject?> GetEventsByCity(string city)
        {
            var url = $"events.json?apikey={_apiKey}&city={city}";
            return await GetAsync<RootObject>(url);
        }
        public async Task<RootObject?> GetEventsByGenreAndCountry(string countryCode, string genreId)
        {
            var url = $"events.json?apikey={_apiKey}&countryCode={countryCode}&genreId={genreId}";
            return await GetAsync<RootObject>(url);
        }
    }
}