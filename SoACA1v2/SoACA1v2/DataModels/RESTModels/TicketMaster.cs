using System.Text.Json.Serialization;

namespace SoACA1v2.DataModels
{
    public class RootObject
    {
        [JsonPropertyName("_embedded")]
        public Embedded Embedded { get; set; }
    }

    public class Embedded
    {
        [JsonPropertyName("events")]
        public Events[] Events { get; set; }
    }

    public class Events
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("test")]
        public bool Test { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("locale")]
        public string Locale { get; set; }

        [JsonPropertyName("images")]
        public Images[] Images { get; set; }

        [JsonPropertyName("dates")]
        public Dates Dates { get; set; }

        [JsonPropertyName("classifications")]
        public Classifications[] Classifications { get; set; }

        [JsonPropertyName("info")]
        public string Info { get; set; }

        [JsonPropertyName("pleaseNote")]
        public string PleaseNote { get; set; }

        [JsonPropertyName("_embedded")]
        public EventEmbedded Embedded { get; set; } 
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

        [JsonPropertyName("country")]
        public Country Country { get; set; }

        [JsonPropertyName("location")]
        public Location Location { get; set; }

        [JsonPropertyName("images")]
        public Images[] Images { get; set; }

        [JsonPropertyName("address")]
        public Address Address { get; set; }
    }

    public class Address
    {
        [JsonPropertyName("line1")]
        public string Line1 { get; set; }
    }

    public class Images
    {
        [JsonPropertyName("ratio")]
        public string Ratio { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("width")]
        public int Width { get; set; }

        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("fallback")]
        public bool Fallback { get; set; }
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

    public class Classifications
    {
        [JsonPropertyName("segment")]
        public Segment Segment { get; set; }
    }

    public class Segment
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class City
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class Country
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("countryCode")]
        public string CountryCode { get; set; }
    }

    public class Location
    {
        [JsonPropertyName("longitude")]
        public string Longitude { get; set; }

        [JsonPropertyName("latitude")]
        public string Latitude { get; set; }
    }
}
