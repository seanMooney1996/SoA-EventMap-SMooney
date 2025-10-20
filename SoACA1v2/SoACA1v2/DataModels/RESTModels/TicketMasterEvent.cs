using System.Text.Json.Serialization;

namespace SoACA1v2.DataModels
{
    public class Event :IComparable<Event>
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("images")]
        public Images[] Images { get; set; }

        [JsonPropertyName("dates")]
        public Dates Dates { get; set; }

        [JsonPropertyName("_embedded")]
        public EventEmbedded Embedded { get; set; } 
        
        public int CompareTo(Event other)
        {
            if (other == null) return 1;
            
            DateTime.TryParse(Dates?.Start?.LocalDate, out DateTime thisDate);
            DateTime.TryParse(other.Dates?.Start?.LocalDate, out DateTime otherDate);
            
            int dateComparison = thisDate.CompareTo(otherDate);
            if (dateComparison != 0)
                return dateComparison;
            
            return string.Compare(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        }
    }
    
    public class RootObject
    {
        [JsonPropertyName("_embedded")]
        public Embedded Embedded { get; set; }
    }

    public class Embedded
    {
        [JsonPropertyName("events")]
        public Event[] Events { get; set; }
    }

    public class EventEmbedded
    {
        [JsonPropertyName("venues")]
        public Venue[] Venues { get; set; }
    }

    public class Venue
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("city")]
        public City City { get; set; }

        [JsonPropertyName("location")]
        public Location Location { get; set; }

        [JsonPropertyName("images")]
        public Images[] Images { get; set; }
        
    }

    public class Images
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    public class Dates
    {
        [JsonPropertyName("start")]
        public Start Start { get; set; }
    }

    public class Start
    {
        [JsonPropertyName("localDate")]
        public string LocalDate { get; set; }

        [JsonPropertyName("localTime")]
        public string LocalTime { get; set; }
    }

    public class City
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class Location
    {
        [JsonPropertyName("longitude")]
        public string Longitude { get; set; }

        [JsonPropertyName("latitude")]
        public string Latitude { get; set; }
    }
}
