using System.Globalization;
using BlazorBootstrap;
using NUnit.Framework.Internal.Execution;
using SoACA1v2.DataModels;

namespace SoACA1v2.Services.StateManagement;
//Map State representing the state inside the google map component
public class MapStateService
{
    private bool isLoading;
    private List<GoogleMapMarker>? markers = null;
    private GoogleMapCenter? googleMapCenter = new GoogleMapCenter(11,11);
    private string? errorMessage;
    private string? zoom = "8";

    public string Zoom
    {
        get => zoom;
        set
        {
            zoom = value;
            Console.WriteLine($"set zoom to {zoom}");
            NotifyStateChanged();
        }
    }
    public bool IsLoading
    {
        get => isLoading;
        set
        {
            isLoading = value;
            Console.WriteLine($"set loading to {isLoading}");
            NotifyStateChanged();
        }
    }

    public List<GoogleMapMarker>? Markers
    {
        get => markers;
        set
        {
            markers = value;
            Console.WriteLine($"set markers to {markers.Count}");
            NotifyStateChanged();
        }
    }

    public GoogleMapCenter? GoogleMapCenter
    {
        get => googleMapCenter;
        set
        {
            googleMapCenter = value;
            Console.WriteLine($"set googlemapCenter  to {googleMapCenter.Latitude},{googleMapCenter.Longitude}");
            NotifyStateChanged();
        }
    }
    
    public event Action? OnChange;
    private void NotifyStateChanged() => OnChange?.Invoke();
}