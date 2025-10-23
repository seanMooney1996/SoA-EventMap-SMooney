using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using SoACA1v2.DataModels;
using SoACA1v2.Services;
using SoACA1v2.Services.HTTP;

namespace SoCA1v2Test;

public class GooglePlacesClientTest
{

    // create a mock version of the configuration to be passed to GooglePlacesClient
    private IConfiguration CreateMockConfig()
    {
        var settings = new Dictionary<string, string?>
        {
            { "Keys:GoogleAPI", "fakeapikey" }
        };
        return new ConfigurationBuilder().AddInMemoryCollection(settings).Build();
    }
    // Create a mock version of a http client to be passed to GooglePlacesClient
    private HttpClient CreateMockHttpClient<T>(T responseObject)
    {
        var handler = new Mock<HttpMessageHandler>();
        handler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()   
            ).ReturnsAsync(() => new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(responseObject))
            });
        return new HttpClient(handler.Object)
        {
            BaseAddress = new Uri("https://places.googleapis.com/")
        };
    }
    
    [Fact]
    public async Task GetVenueByName_ReturnsResponse()
    {
        // Arrange - Mock the response object 
        var fakeResponse = new GoogleLocations.Root
        {
            Results = new List<GoogleLocations.Result>
            {
                new GoogleLocations.Result
                {
                    Name = "Test Venue",
                    Geometry = new GoogleLocations.Geometry
                    {
                        Location = new GoogleLocations.Location
                        {
                            Lat = 52.0,
                            Lng = -8.0
                        }
                    }
                }
            }
        };

        var httpClient = CreateMockHttpClient(fakeResponse);
        var config = CreateMockConfig();
        var client = new GooglePlacesClient(httpClient, config);

        // Act - Call the mocked client for result
        var result = await client.GetVenueByName("Test Venue");

        // Assert - ensure response is not null
        Assert.NotNull(result);
    }
}