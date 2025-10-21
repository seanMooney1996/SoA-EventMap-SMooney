using System.Globalization;
using BlazorBootstrap;
using LeafletForBlazor.RealTime.polygons;
using SoACA1v2.DataModels;
using SoACA1v2.Services.Interfaces;
using SoACA1v2.Services.StateManagement;

namespace SoACA1v2.Services.Controller;

public class EventController : IDisposable
{
    private readonly EventStateService _eventsState;
    private readonly MapStateService _mapState;
    private readonly IGooglePlacesClient _googlePlacesClient;
    private CancellationTokenSource? _debounceCts;
    private bool _isFetchingVenues = false;
    
    public EventController(
        EventStateService eventsState,
        MapStateService mapState,
        IGooglePlacesClient googlePlacesClient)
    {
        Console.WriteLine("Initializing event controller state");
        _eventsState = eventsState;
        _googlePlacesClient = googlePlacesClient;
        _mapState = mapState;
        _eventsState.OnEventsChanged += OnEventStateChanged;
        _eventsState.EventToLocateChanged += OnEventToLocateChanged;
    }

    private void OnEventToLocateChanged()
    {
        Console.WriteLine("EventToLocateChanged");
        _mapState.IsLoading = true;
        int? index = _eventsState.EventToLocateIndex;
        if (index.HasValue && _mapState.Markers != null)
        {
            Console.WriteLine("index value: " + index.Value);
            if (_eventsState?.Events != null)
            {
                var foundEvent = _eventsState.Events.ElementAt(index.Value);
                var lat = foundEvent.Embedded.Venues[0].Location.Latitude;
                var lon = foundEvent.Embedded.Venues[0].Location.Longitude;
                    Console.WriteLine("marker pos " + lat +  "," + lon);
                    _mapState.GoogleMapCenter = new GoogleMapCenter(Double.Parse(lat)+0.0005, Double.Parse(lon));
                    _mapState.Zoom = "18";
            }
        }
        _mapState.IsLoading = false;
    }
    private async void OnEventStateChanged()
    {
        try
        {
            await FetchVenuesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to retrieve venues: {ex.Message}");
        }
    }
    
    private async Task FetchVenuesAsync()
    {
        _mapState.IsLoading = true;
        Console.WriteLine("Fetching All Venues...");
        List<GoogleMapMarker> mapMarkers = new();
        if (_eventsState.Events != null)
            foreach (var eventsStateEvent in _eventsState.Events)
            {
                var ven = eventsStateEvent.Embedded.Venues[0];
                try
                {
                    var data = await _googlePlacesClient.GetVenueByName(
                        ven.Name,
                        ven.Location.Latitude,
                        ven.Location.Longitude
                    );

                    if (data != null)
                    {
                        GoogleMapMarker? marker = CreateMarkerFromEvent(eventsStateEvent, data);
                        if (marker != null)
                            mapMarkers.Add(marker);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"Error fetching locations/venues for {eventsStateEvent.Embedded.Venues[0].Name} : {ex.Message}");
                }
            }

        _mapState.Markers = mapMarkers;
        var (lat, lng, zoom) = GetCenterAndZoom(mapMarkers);
        if (double.IsNaN(lat) || double.IsNaN(lng))
        {
            _mapState.GoogleMapCenter = new GoogleMapCenter(12,12);
            _mapState.Zoom = "3";
        }
        else
        {
            _mapState.GoogleMapCenter = new GoogleMapCenter(lat+1,lng);
            _mapState.Zoom = zoom.ToString();
        }
        _mapState.IsLoading = false; 
    }
    
    // Partse event and location data into marker for maps
     private static GoogleMapMarker? CreateMarkerFromEvent(Event ev, GoogleLocations.Root? locationResult)
    {
        string eventName = ev.Name ?? "Unnamed Event";
        string date = ev.Dates?.Start?.LocalDate ?? "TBA";
        string time = ev.Dates?.Start?.LocalTime ?? "";
        string eventImage = ev.Images?.FirstOrDefault()?.Url ?? "https://via.placeholder.com/180x100?text=No+Event+Image";
        string venueName = "Venue TBA";
        string city = "";
        string? venueImage = "https://via.placeholder.com/180x80?text=No+Venue+Image";
        double lat = 0, lng = 0;
        if (ev.Embedded?.Venues != null && ev.Embedded.Venues.Length > 0)
        {
            var venue = ev.Embedded.Venues[0];
            venueName = venue?.Name ?? venueName;
            city = venue?.City?.Name ?? city;
            venueImage = venue?.Images?.FirstOrDefault()?.Url ?? venueImage;

            if (venue?.Location != null &&
                double.TryParse(venue.Location.Latitude, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsedLat) &&
                double.TryParse(venue.Location.Longitude, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsedLng))
            {
                lat = parsedLat;
                lng = parsedLng;
            }
        }
        if (locationResult?.Results is { Count: > 0 })
        {
            var googleResult = locationResult.Results.First();
            
            if (venueName == "Venue TBA" && !string.IsNullOrEmpty(googleResult.Name))
                venueName = googleResult.Name;
            
            if (googleResult.Geometry?.Location != null)
            {
                double gLat = googleResult.Geometry.Location.Lat;
                double gLng = googleResult.Geometry.Location.Lng;
                if (lat == 0 && lng == 0)
                {
                    lat = gLat;
                    lng = gLng;
                }
            }
            if (googleResult.Photos.Count > 0)
            {
                venueImage = googleResult.Photos.First().HtmlAttributions?.FirstOrDefault();
            }
        }
        var content = $@"
            <div class='p-2 bg-white rounded shadow-sm text-center' style='width:200px'>
                <img src='{eventImage}' class='rounded mb-1' alt='{eventName}' 
                     style='width:100%; height:100px; object-fit:cover;'/>
                <div class='fw-bold text-dark'>{eventName}</div>
                <div class='text-muted small'>{date} - {time}</div>
                <img src='{venueImage}' class='rounded mb-2' alt='{venueName}' 
                     style='width:100%; height:80px; object-fit:cover; opacity:0.9;'/>
                <div class='text-muted small'>{city}</div>
                <div class='text-muted small'> {venueName}</div>
                <a href='{ev.Url}' target='_blank' class='small text-primary'>View Details</a>
            </div>";

        return new GoogleMapMarker
        {
            Position = new GoogleMapMarkerPosition(lat, lng),
            Title = $"{eventName} - {venueName}",
            Content = content
        };
    }
    
   // https://stackoverflow.com/questions/35645423/how-to-get-center-pointlatlng-between-some-locations
   // on how to figure out center lat long in list of lat long
   // To find the center we just find the halfway point between both the min and max of both the latitudes and longitudes for each map marker.
   // To work out zoom level we find the biggest difference among longs and lats, and mapped a zoom level based on what that distance was using the switch.
   private (double lat, double longt, int zoom) GetCenterAndZoom(List<GoogleMapMarker> markers)
   {
       double minLat = double.MaxValue, maxLat = double.MinValue;
       double minLng = double.MaxValue, maxLng = double.MinValue;

       foreach (var m in markers)
       {
           if (m.Position == null)
           {
               continue;
           }
           minLat = Math.Min(minLat, m.Position.Latitude);
           maxLat = Math.Max(maxLat, m.Position.Latitude);
           minLng = Math.Min(minLng, m.Position.Longitude);
           maxLng = Math.Max(maxLng, m.Position.Longitude);
       }
       double centerLat = (minLat + maxLat) / 2.0;
       double centerLng = (minLng + maxLng) / 2.0;
       double latDiff = maxLat - minLat;
       double lngDiff = maxLng - minLng;
       double maxDiff = Math.Max(latDiff, lngDiff);
       
       Console.WriteLine("max dif for lat and longs - "+maxDiff);

       int zoom = maxDiff switch
       {
           > 40 => (int)Zoom.SHORTEST,   
           > 20 => (int)Zoom.SHORT,
           > 10 => (int)Zoom.LOWERMEDIUM,
           > 5 => (int)Zoom.UPPERMEDIUM,
           > 2 => (int)Zoom.LONG,
            _ => (int)Zoom.LONGEST,
       };
       Console.WriteLine($"New center - {centerLat}, {centerLng} - zoom: " + zoom);
       return (centerLat, centerLng, zoom);
   }

    public void Dispose()
    {
        _debounceCts?.Cancel();
        _debounceCts?.Dispose();
        _eventsState.OnEventsChanged -= OnEventStateChanged;
    }
    
    

    private enum Zoom
    {
        SHORTEST=2,SHORT=3,LOWERMEDIUM=4,UPPERMEDIUM=5,LONG=6,LONGEST=7
    }
}