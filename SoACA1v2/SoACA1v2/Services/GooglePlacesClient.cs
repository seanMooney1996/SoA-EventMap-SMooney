namespace SoACA1v2.Services;

using SoACA1v2.DataModels;
using System.Net.Http.Json;

public class GooglePlacesClient : HttpClientBase
{
    public GooglePlacesClient(HttpClient http, IConfiguration config)
        : base(http, config, "GoogleAPI") { }
    
    public async Task<RootObject?> TestSearch(List<string> queryList)
    {
        var query = queryList[0];
        for (int i = 1; i < queryList.Count; i++)
        {
            if (i != queryList.Count - 1)
            {
                query += queryList[i] + "+"; 
            }
        }
        var url = $"https://maps.googleapis.com/maps/api/place/textsearch/json??query={query}&key={_apiKey}";
        return await GetAsync<RootObject>(url);
    }
    
}