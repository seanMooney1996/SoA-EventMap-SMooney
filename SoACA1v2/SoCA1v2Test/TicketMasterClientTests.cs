using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using SoACA1v2.DataModels;
using SoACA1v2.Services;
using SoACA1v2.Services.HTTP;

namespace SoCA1v2Test;

public class TicketMasterClientTests
{
    // using moq library here to mock desired results from calls and c# library objects
     // create a mock version of the configuration to be passed to GooglePlacesClient
    private IConfiguration CreateMockConfig()
    {
        var settings = new Dictionary<string, string?>
        {
            { "Keys:TicketMasterAPI", "fakeapikey" }
        };
        return new ConfigurationBuilder().AddInMemoryCollection(settings).Build();
    }
    // Create a mock version of a http client to be passed to GooglePlacesClient
    private HttpClient CreateMockHttpClient<T>(T responseObject,Mock<HttpMessageHandler> mockHandler)
    {
        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()   
            ).ReturnsAsync(() => new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(responseObject))
            });
        return new HttpClient(mockHandler.Object)
        {
            BaseAddress = new Uri("https://app.ticketmaster.com/discovery/v2/")
        };
    }
    
    [Fact]
    public async Task GetEvents_ReturnsResponse()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        var httpClient = CreateMockHttpClient(fakeResponse,handlerMock);
        var config = CreateMockConfig();
        var client = new TicketMasterClient(httpClient, config);

        // Act
        var result = await client.GetEvents("test", null, null, null);

        // Assert
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task GetEvents_AddsDefaultCountry_WhenAllOptionalParamsMissing()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();
        var config = CreateMockConfig();

        var httpClient = CreateMockHttpClient(fakeResponse,handlerMock);
        var client = new TicketMasterClient(httpClient, config);
        
        // Act
        await client.GetEvents(null, null, null, null);

        // Assert - Check that uri contains defauot value IE
        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri != null && req.RequestUri.ToString().Contains("countryCode=IE")),
        ItExpr.IsAny<CancellationToken>()
        );
    }
    
    [Fact]
    public async Task GetEvents_RequestContainsRequiredParams_WhenPassed()
    {
        // Arrange
        var handlerMock = new Mock<HttpMessageHandler>();

        var httpClient = new HttpClient(handlerMock.Object)
        {
            BaseAddress = new Uri("https://app.ticketmaster.com/discovery/v2/")
        };

        var config = CreateMockConfig();
        var client = new TicketMasterClient(httpClient, config);

        // Act
        await client.GetEvents("US", "rock", new DateOnly(2025, 10, 21), new DateOnly(2025, 10, 21));

        // Assert - Check that the request uri contains all passed params
        handlerMock.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.RequestUri != null &&
                req.RequestUri.ToString().Contains("apikey=fakeapikey") &&
                req.RequestUri.ToString().Contains("countryCode=US") &&
                req.RequestUri.ToString().Contains("genreId=rock") &&
                req.RequestUri.ToString().Contains("classificationName=Music") &&
                req.RequestUri.ToString().Contains("startDateTime=2025-10-21") &&
                req.RequestUri.ToString().Contains("endDateTime=2025-10-21")),
            ItExpr.IsAny<CancellationToken>()
        );
    }

    RootObject fakeResponse = new RootObject
    {
        Embedded = new Embedded
        {
            Events = new[]
            {
                new Event
                {
                    Name = "test event",
                    Url = "testurl",
                    Dates = new Dates
                    {
                        Start = new Start
                        {
                            LocalDate = "testdate",
                            LocalTime = "testtime"
                        }
                    },
                    Images = new[]
                    {
                        new Images { Url = "test" },
                        new Images { Url = "test" }
                    },
                    Embedded = new EventEmbedded
                    {
                        Venues = new[]
                        {
                            new Venue
                            {
                                Name = "test Venue",
                                City = new City { Name = "test" },
                                Location = new Location
                                {
                                    Latitude = "1.1",
                                    Longitude = "1.1"
                                },
                                Images = new[]
                                {
                                    new Images { Url = "test" }
                                }
                            }
                        }
                    }
                }
            }
        }
    };
}