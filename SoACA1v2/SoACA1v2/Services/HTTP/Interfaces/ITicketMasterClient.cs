using SoACA1v2.DataModels;

namespace SoACA1v2.Services.HTTP.Interfaces;

public interface ITicketMasterClient
{
    Task<RootObject?> GetEvents(string? countryCode, string? genreId, DateOnly? startDate, DateOnly? endDate, string? keyword = null);
}