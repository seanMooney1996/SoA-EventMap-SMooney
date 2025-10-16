using SoACA1v2.DataModels;
using SoACA1v2.Services.StateManagement;

namespace SoACA1v2.Services.Controller;

public class TicketMasterController : IDisposable
{
    private readonly TicketMasterSearchStateService _searchState;
    private readonly TicketMasterEventStateService _eventsState;
    private readonly TicketMasterClient _client;
    private CancellationTokenSource? _debounceCts;

    public TicketMasterController(
        TicketMasterSearchStateService searchState,
        TicketMasterEventStateService eventsState,
        TicketMasterClient client)
    {
        Console.WriteLine("Initializing controller state");
        _searchState = searchState;
        _eventsState = eventsState;
        _client = client;
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
            await Task.Delay(500, token);
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

            var data = await _client.GetEvents(
                _searchState.SelectedCountry.Code,
                _searchState.SelectedGenre.Id,
                _searchState.StartDate,
                _searchState.EndDate,
                _searchState.Keywords);

            var events = data?._embedded?.events?.ToList() ?? new List<Events>();
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
