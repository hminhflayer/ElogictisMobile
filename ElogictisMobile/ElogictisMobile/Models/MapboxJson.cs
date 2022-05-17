namespace ElogictisMobile.Models
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class MapBoxJson
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("waypoints")]
        public List<Waypoint> Waypoints { get; set; }

        [JsonProperty("trips")]
        public List<Trip> Trips { get; set; }
    }

    public partial class Trip
    {
        [JsonProperty("geometry")]
        public string Geometry { get; set; }

        [JsonProperty("legs")]
        public List<Leg> Legs { get; set; }

        [JsonProperty("weight_name")]
        public string WeightName { get; set; }

        [JsonProperty("weight")]
        public double Weight { get; set; }

        [JsonProperty("duration")]
        public double Duration { get; set; }

        [JsonProperty("distance")]
        public double Distance { get; set; }
    }

    public partial class Leg
    {
        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("weight")]
        public double Weight { get; set; }

        [JsonProperty("duration")]
        public double Duration { get; set; }

        [JsonProperty("steps")]
        public List<object> Steps { get; set; }

        [JsonProperty("distance")]
        public double Distance { get; set; }
    }

    public partial class Waypoint
    {
        [JsonProperty("distance")]
        public double Distance { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("location")]
        public List<double> Location { get; set; }

        [JsonProperty("waypoint_index")]
        public long WaypointIndex { get; set; }

        [JsonProperty("trips_index")]
        public long TripsIndex { get; set; }
    }
}
