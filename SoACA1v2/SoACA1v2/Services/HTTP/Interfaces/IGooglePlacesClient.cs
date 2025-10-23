using SoACA1v2.DataModels;

namespace SoACA1v2.Services.HTTP.Interfaces;

public interface IGooglePlacesClient
{
    Task<GoogleLocations.Root?> GetVenueByName(string venueName, string? latitude = null, string? longitude = null);
}