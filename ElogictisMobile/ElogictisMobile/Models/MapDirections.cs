namespace ElogictisMobile.Models
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class MapDirections
    {
        [JsonProperty("meta")]
        public Meta Meta { get; set; }

        [JsonProperty("response")]
        public Response Response { get; set; }
    }

    public partial class Meta
    {
        [JsonProperty("server_time")]
        public DateTimeOffset ServerTime { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public partial class Response
    {
        [JsonProperty("directions")]
        public Directions Directions { get; set; }

        [JsonProperty("found")]
        public bool Found { get; set; }

        [JsonProperty("origin")]
        public string Origin { get; set; }

        [JsonProperty("destinations")]
        public List<string> Destinations { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("distance_units")]
        public string DistanceUnits { get; set; }

        [JsonProperty("avoid_routes")]
        public List<string> AvoidRoutes { get; set; }

        [JsonProperty("include_timed_distance")]
        public bool IncludeTimedDistance { get; set; }

        [JsonProperty("expand_routes")]
        public bool ExpandRoutes { get; set; }
    }

    public partial class Directions
    {
        [JsonProperty("origin")]
        public Origin Origin { get; set; }

        [JsonProperty("destinations")]
        public List<Origin> Destinations { get; set; }

        [JsonProperty("routes")]
        public List<Route> Routes { get; set; }

        [JsonProperty("directions_link")]
        public Uri DirectionsLink { get; set; }

        [JsonProperty("request_timestamp")]
        public long RequestTimestamp { get; set; }
    }

    public partial class Origin
    {
        [JsonProperty("place_google_id")]
        public string PlaceGoogleId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("full_address")]
        public string FullAddress { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("query")]
        public string Query { get; set; }
    }

    public partial class Route
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("distance_meters")]
        public long DistanceMeters { get; set; }

        [JsonProperty("duration_seconds")]
        public long DurationSeconds { get; set; }

        [JsonProperty("duration_label")]
        public string DurationLabel { get; set; }

        [JsonProperty("min_duration_seconds")]
        public long MinDurationSeconds { get; set; }

        [JsonProperty("max_duration_seconds")]
        public long MaxDurationSeconds { get; set; }

        [JsonProperty("route_parts")]
        public List<RoutePart> RouteParts { get; set; }

        [JsonProperty("distance_label")]
        public string DistanceLabel { get; set; }

        [JsonProperty("departure_timestamp")]
        public long DepartureTimestamp { get; set; }

        [JsonProperty("departure_datetime_utc")]
        public DateTimeOffset DepartureDatetimeUtc { get; set; }

        [JsonProperty("arrival_timestamp")]
        public long ArrivalTimestamp { get; set; }

        [JsonProperty("arrival_datetime_utc")]
        public DateTimeOffset ArrivalDatetimeUtc { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class RoutePart
    {
        [JsonProperty("distance_meters")]
        public long DistanceMeters { get; set; }

        [JsonProperty("duration_seconds")]
        public long DurationSeconds { get; set; }

        [JsonProperty("duration_label")]
        public string DurationLabel { get; set; }

        [JsonProperty("min_duration_seconds")]
        public long MinDurationSeconds { get; set; }

        [JsonProperty("max_duration_seconds")]
        public long MaxDurationSeconds { get; set; }

        [JsonProperty("steps")]
        public List<Step> Steps { get; set; }

        [JsonProperty("distance_label")]
        public string DistanceLabel { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("destination_timezone")]
        public string DestinationTimezone { get; set; }

        [JsonProperty("destination_timezone_short")]
        public string DestinationTimezoneShort { get; set; }
    }

    public partial class Step
    {
        [JsonProperty("distance_meters")]
        public long DistanceMeters { get; set; }

        [JsonProperty("duration_seconds")]
        public long DurationSeconds { get; set; }

        [JsonProperty("duration_label")]
        public string DurationLabel { get; set; }

        [JsonProperty("distance_label")]
        public string DistanceLabel { get; set; }
    }
}
