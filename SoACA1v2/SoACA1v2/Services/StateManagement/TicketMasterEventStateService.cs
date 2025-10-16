using SoACA1v2.DataModels;

namespace SoACA1v2.Services.StateManagement;

public class TicketMasterEventStateService
{
        private bool isLoading;
        private List<Events>? events;
        private string? errorMessage;

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
        
}