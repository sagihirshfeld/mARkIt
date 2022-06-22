using System;
using Newtonsoft.Json;

namespace mARkIt.Abstractions
{
    public abstract class TableData
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string Version { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Deleted { get; set; }
    }
}
