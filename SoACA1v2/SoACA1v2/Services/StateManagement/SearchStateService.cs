using SoACA1v2.DataModels;

namespace SoACA1v2.Services.StateManagement
{
    public class SearchStateService
    {
        private GenreItem selectedGenre = new();
        private CountryItem selectedCountry = new();
        private DateOnly startDate = DateOnly.FromDateTime(DateTime.Now);
        private DateOnly endDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(3));
        private string keywords = String.Empty;

        public string Keywords
        {
            get => keywords;
            set
            {
                keywords = value;
                NotifyStateChanged();
            }
        }
        public GenreItem SelectedGenre
        {
            get => selectedGenre;
            set
            {
                selectedGenre = value;
                NotifyStateChanged();
            }
        }
        public CountryItem SelectedCountry
        {
            get => selectedCountry;
            set
            {
                selectedCountry = value;
                NotifyStateChanged();
            }
        }
        public DateOnly StartDate
        {
            get => startDate;
            set
            {
                startDate = value;
                NotifyStateChanged();
            }
        }
        public DateOnly EndDate
        {
            get => endDate;
            set
            {
                endDate = value;
                NotifyStateChanged();
            }
        }
        public event Action? OnChange;
        private void NotifyStateChanged()
        {
            Console.WriteLine("NotifyStateChanged in search state");
            OnChange?.Invoke();
        }
    }
}