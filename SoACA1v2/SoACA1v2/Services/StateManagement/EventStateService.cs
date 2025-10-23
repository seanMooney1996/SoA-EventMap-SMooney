using SoACA1v2.DataModels;

namespace SoACA1v2.Services.StateManagement;

// state object representing state in the event layout
public class EventStateService
{
        private bool isLoading;
        private List<Event>? events;
        private string? errorMessage;
        private int? eventToLocateIndex;
        
        // these 'events' are subscribed to. Anything subscribed to these events are notified on a change.
        public event Action? OnEventsChanged;
        public event Action? EventToLocateChanged;
        public event Action? OnChange;

        public int? EventToLocateIndex
        {
            get => eventToLocateIndex;
            set
            {
                eventToLocateIndex = value;
                NotifyEventSelected();
            }
        }
        public bool IsLoading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                NotifyStateChanged();
            }
        }

        public List<Event>? Events
        {
            get => events;
            set
            {
                events = value;
                OnEventsChanged?.Invoke();
                NotifyStateChanged();
            }
        }

        public string? ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                NotifyStateChanged();
            }
        }


        private void NotifyStateChanged() => OnChange?.Invoke();
        
        private void NotifyEventSelected() =>  EventToLocateChanged?.Invoke();
        
}