using SoACA1v2.DataModels;

namespace SoACA1v2.Services.StateManagement;

public class TicketMasterSearchState
{
    public GenreItem? SelectedGenre { get; set; }
    public CountryItem? SelectedCountry { get; set; }
    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly EndDate { get; set; } = DateOnly.FromDateTime(DateTime.Now.AddMonths(6));
}