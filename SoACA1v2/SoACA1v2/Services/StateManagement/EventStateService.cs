using SoACA1v2.DataModels;

namespace SoACA1v2.Services.StateManagement;

public class EventStateService
{
        private bool isLoading;
        private List<Events>? events;
        private string? errorMessage;
        private int? eventToLocateIndex;
        public event Action? OnEventsChanged;
        
        public event Action? EventToLocateChanged;

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

        public List<Events>? Events
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

        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();
        
        private void NotifyEventSelected() =>  EventToLocateChanged?.Invoke();
        
}