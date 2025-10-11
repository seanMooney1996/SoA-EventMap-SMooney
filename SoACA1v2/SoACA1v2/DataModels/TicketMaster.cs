using System.Text.Json.Serialization;

namespace SoACA1v2.DataModels
{
    
    public class RootObject
    {
        [JsonPropertyName("_embedded")]
        public _embedded _embedded { get; set; }
    }

    public class _embedded
    {
        [JsonPropertyName("events")]
        public Events[] events { get; set; }
    }

    public class Events
    {
        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("type")]
        public string type { get; set; }

        [JsonPropertyName("id")]
        public string id { get; set; }

        [JsonPropertyName("test")]
        public bool test { get; set; }

        [JsonPropertyName("url")]
        public string url { get; set; }

        [JsonPropertyName("locale")]
        public string locale { get; set; }

        [JsonPropertyName("images")]
        public Images[] images { get; set; }

        [JsonPropertyName("dates")]
        public Dates dates { get; set; }

        [JsonPropertyName("classifications")]
        public Classifications[] classifications { get; set; }

        [JsonPropertyName("info")]
        public string info { get; set; }

        [JsonPropertyName("pleaseNote")]
        public string pleaseNote { get; set; }
    }

    public class Images
    {
        [JsonPropertyName("ratio")]
        public string ratio { get; set; }

        [JsonPropertyName("url")]
        public string url { get; set; }

        [JsonPropertyName("width")]
        public int width { get; set; }

        [JsonPropertyName("height")]
        public int height { get; set; }

        [JsonPropertyName("fallback")]
        public bool fallback { get; set; }
    }

    public class Public
    {
        [JsonPropertyName("startDateTime")]
        public string startDateTime { get; set; }

        [JsonPropertyName("startTBD")]
        public bool startTBD { get; set; }

        [JsonPropertyName("startTBA")]
        public bool startTBA { get; set; }

        [JsonPropertyName("endDateTime")]
        public string endDateTime { get; set; }
    }

    public class Dates
    {
        [JsonPropertyName("start")]
        public Start start { get; set; }
    }

    public class Start
    {
        [JsonPropertyName("localDate")]
        public string localDate { get; set; }

        [JsonPropertyName("localTime")]
        public string localTime { get; set; }
    }

    public class Classifications
    {
        [JsonPropertyName("segment")]
        public Segment segment { get; set; }
    }

    public class Segment
    {
        [JsonPropertyName("id")]
        public string id { get; set; }

        [JsonPropertyName("name")]
        public string name { get; set; }
    }

    public class City
    {
        [JsonPropertyName("name")]
        public string name { get; set; }
    }

    public class Country
    {
        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("countryCode")]
        public string countryCode { get; set; }
    }

    public class Location
    {
        [JsonPropertyName("longitude")]
        public string longitude { get; set; }

        [JsonPropertyName("latitude")]
        public string latitude { get; set; }
    }

    public class ExternalLinks
    {
        [JsonPropertyName("twitter")]
        public Twitter[] twitter { get; set; }

        [JsonPropertyName("facebook")]
        public Facebook[] facebook { get; set; }

        [JsonPropertyName("wiki")]
        public Wiki[] wiki { get; set; }

        [JsonPropertyName("instagram")]
        public Instagram[] instagram { get; set; }

        [JsonPropertyName("homepage")]
        public Homepage[] homepage { get; set; }
    }

    public class Twitter
    {
        [JsonPropertyName("url")]
        public string url { get; set; }
    }

    public class Facebook
    {
        [JsonPropertyName("url")]
        public string url { get; set; }
    }

    public class Wiki
    {
        [JsonPropertyName("url")]
        public string url { get; set; }
    }

    public class Instagram
    {
        [JsonPropertyName("url")]
        public string url { get; set; }
    }

    public class Homepage
    {
        [JsonPropertyName("url")]
        public string url { get; set; }
    }
}
