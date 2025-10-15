namespace SoACA1v2.Services.StateManagement;

public class TicketMasterStateService
{
    private TicketMasterSearchState? _ticketMasterSearchState = new();

    public TicketMasterSearchState? TicketMasterSearchState
    {
        get => _ticketMasterSearchState;
        set
        {
            if (_ticketMasterSearchState != value)
            {
                _ticketMasterSearchState = value;
                OnChange?.Invoke();
            }
        }
    }
    public void UpdateState(Action<TicketMasterSearchState> updateAction)
    {
        updateAction(_ticketMasterSearchState);
        NotifyStateChanged();
    }
    
    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();
    
}