using System.Text;
using System.Text.Json;
using SoACA1v2.Services.Interfaces;

namespace SoACA1v2.Services;

using SoACA1v2.DataModels;
using System.Net.Http.Json;

public class GooglePlacesClient : HttpClientBase, IGooglePlacesClient
{
    public GooglePlacesClient(HttpClient http, IConfiguration config)
        : base(http, config, "GoogleAPI") { }
    
    public async Task<GoogleLocations.Root?> GetVenueByName(string venueName, string? latitude = null, string? longitude = null)
    {
        if (string.IsNullOrWhiteSpace(venueName))
            return null;
        var endpoint = "/v1/places:searchText";

        object requestBody;
        if (!string.IsNullOrEmpty(latitude) && !string.IsNullOrEmpty(longitude))
        {
            requestBody = new
            {
                textQuery = venueName,
                locationBias = new
                {
                    circle = new
                    {
                        center = new
                        {
                            latitude = double.Parse(latitude),
                            longitude = double.Parse(longitude)
                        },
                        radius = 5000.0
                    }
                }
            };
        }
        else
        {
            requestBody = new { textQuery = venueName };
        }
        
        var json = JsonSerializer.Serialize(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        content.Headers.Add("X-Goog-Api-Key", _apiKey);
        content.Headers.Add("X-Goog-FieldMask", "places.displayName,places.formattedAddress,places.location,places.id,places.photos,places.rating,places.userRatingCount,places.types");
        
        return await GetAsyncWithContent<GoogleLocations.Root>(endpoint, content);
    }
    
}