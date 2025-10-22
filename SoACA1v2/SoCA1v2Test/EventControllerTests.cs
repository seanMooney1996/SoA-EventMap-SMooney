using System.Net;
using BlazorBootstrap;
using Moq;
using Moq.Protected;
using SoACA1v2.DataModels;
using SoACA1v2.Services.Controller;
using SoACA1v2.Services.Interfaces;
using SoACA1v2.Services.StateManagement;
using SoCA1v2Test.fakeObjects;

namespace SoCA1v2Test;

public class EventControllerTests
{
    [Fact]
    public void OnEventToLocateChanged_UpdatesMapCenterAndZoom_WhenEventSelected()
    {
        // Arrange - Set up required services 
        var eventState = new EventStateService();
        var mapState = new MapStateService();
        {
            List<GoogleMapMarker> Markers = new();
        };

        var mockPlacesClient = new Mock<IGooglePlacesClient>();
        var controller = new EventController(eventState, mapState, mockPlacesClient.Object);
        
        eventState.Events = new List<Event> { FakeTicketMasterResponse.FakeResponse.Embedded.Events.First() };
       
        // Act 
        eventState.EventToLocateIndex = 0;
        
        var venue = FakeTicketMasterResponse.FakeResponse.Embedded.Events.First().Embedded.Venues.First();
        // Assert - 
        Assert.False(mapState.IsLoading); 
        Assert.NotNull(mapState.GoogleMapCenter);
        Assert.Equal(double.Parse(venue.Location.Latitude) + 0.0005, mapState.GoogleMapCenter.Latitude);
        Assert.Equal(double.Parse(venue.Location.Longitude), mapState.GoogleMapCenter.Longitude);
        Assert.Equal("18", mapState.Zoom);
    }
    
    [Fact]
    public void FetchVenuesAsync_DoesntCall_GetVenueByName_WhenEventsEmpty()
    {
        // Arrange - Set up required services 
        var eventState = new EventStateService();
        var mapState = new MapStateService();
        
        var mockPlacesClient = new Mock<IGooglePlacesClient>();
        var eventController = new EventController(eventState, mapState, mockPlacesClient.Object);
        // Act - Call FetchVenuesAsync indrectly
        eventState.Events = null;
        // Assert - Verify that GetVenueByName
        mockPlacesClient.Verify(
            c => c.GetVenueByName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Never
        );
        Assert.True(mapState.Markers.Count  == 0);
    }
    
    [Fact]
    public void FetchVenuesAsync_Calls_GetVenueByName_WhenEventsIsFull()
    {
        // Arrange - Set up required services 
        var eventState = new EventStateService();
        var mapState = new MapStateService();
        var mockPlacesClient = new Mock<IGooglePlacesClient>();
        eventState.Events = FakeTicketMasterResponse.FakeResponse.Embedded.Events.ToList();
        
        mockPlacesClient
            .Setup(client => client.GetVenueByName(
                It.IsAny<string>(),
                It.IsAny<string?>(),
                It.IsAny<string?>()))
            .ReturnsAsync(fakeResponse);
        
        var eventController = new EventController(eventState, mapState, mockPlacesClient.Object);
        // Act - Call FetchVenuesAsync indrectly
        eventState.Events = new List<Event> {FakeTicketMasterResponse.FakeResponse.Embedded.Events.First() };
        // Assert - Verify that GetVenueByName
        mockPlacesClient.Verify(
            c => c.GetVenueByName(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
            Times.Once
        );
    }
    
    [Fact]
    public void GetCenterAndZoom_ReturnsCorrectCenterAndZoom_ForSimpleMarkers()
    {
        // Arrange // 
        var eventState = new EventStateService();
        var mapState = new MapStateService();
        
        var mockPlacesClient = new Mock<IGooglePlacesClient>();
        var eventController = new EventController(eventState, mapState, mockPlacesClient.Object);
        var markers = new List<GoogleMapMarker>
        {
            new GoogleMapMarker { Position = new GoogleMapMarkerPosition(10.0,20.0) },
            new GoogleMapMarker { Position = new GoogleMapMarkerPosition(20.0,30.0) },
        };

        // Act
        var (lat, lng, zoom) = eventController.GetCenterAndZoom(markers);

        // Assert
        Assert.Equal(15.0, lat, 3); 
        Assert.Equal(25.0, lng, 3); 
        Assert.Equal(5, zoom);  
    }
    
   public GoogleLocations.Root fakeResponse = new GoogleLocations.Root
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
}