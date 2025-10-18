using SoACA1v2.DataModels;
using SoACA1v2.Services.StateManagement;

namespace SoACA1v2.Services.Controller;

public class SearchStateController : IDisposable
{
    private readonly SearchStateService _searchState;
    private readonly EventStateService _eventsState;
    private readonly TicketMasterClient _ticketMasterClient;
    private CancellationTokenSource? _debounceCts;

    public SearchStateController(
        SearchStateService searchState,
        EventStateService eventsState,
        TicketMasterClient ticketMasterClient)
    {
        Console.WriteLine("Initializing controller state");
        _searchState = searchState;
        _eventsState = eventsState;
        _ticketMasterClient = ticketMasterClient;
        _searchState.OnChange += OnSearchStateChanged;
        _ = FetchEventsAsync();
    }

    private async void OnSearchStateChanged()
    {
        Console.WriteLine($"State changed at {DateTime.Now:HH:mm:ss.fff}");
        _debounceCts?.Cancel();
        _debounceCts = new CancellationTokenSource();
        var token = _debounceCts.Token;
        try
        {
            await Task.Delay(200,token);
            if (!token.IsCancellationRequested)
                await FetchEventsAsync();
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine($"Search timed out");
        }
    }

    private async Task FetchEventsAsync()
    {
        Console.WriteLine("Fetching events...");
        try
        {
            _eventsState.IsLoading = true;

            var data = await _ticketMasterClient.GetEvents(
                _searchState.SelectedCountry.Code,
                _searchState.SelectedGenre.Id,
                _searchState.StartDate,
                _searchState.EndDate,
                _searchState.Keywords);

            var events = data?.Embedded?.Events?.ToList() ?? new List<Events>();
            _eventsState.Events = events;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching events: {ex.Message}");
            _eventsState.ErrorMessage = "An error occurred while searching for events.";
        }
        finally
        {
            _eventsState.IsLoading = false;
        }
    }

    public void Dispose()
    {
        _debounceCts?.Cancel();
        _debounceCts?.Dispose();
        _searchState.OnChange -= OnSearchStateChanged;
    }
}
