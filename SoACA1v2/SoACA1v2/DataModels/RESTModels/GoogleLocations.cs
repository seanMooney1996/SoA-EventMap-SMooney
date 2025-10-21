using System.Text.Json.Serialization;

namespace SoACA1v2.DataModels
{
    public class GoogleLocations
    {
        public class Geometry
        {
            [JsonPropertyName("location")]
            public Location Location { get; set; }
        }

        public class Location
        {
            [JsonPropertyName("lat")]
            public double Lat { get; set; }

            [JsonPropertyName("lng")]
            public double Lng { get; set; }
        }

        public class Northeast
        {
            [JsonPropertyName("lat")]
            public double Lat { get; set; }

            [JsonPropertyName("lng")]
            public double Lng { get; set; }
        }

        public class Photo
        {
            [JsonPropertyName("html_attributions")]
            public List<string> HtmlAttributions { get; set; }
        }

        public class Result
        {

            [JsonPropertyName("geometry")]
            public Geometry Geometry { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("photos")]
            public List<Photo> Photos { get; set; }
        }

        public class Root
        {
            [JsonPropertyName("results")]
            public List<Result> Results { get; set; }
            
        }
    }
}
