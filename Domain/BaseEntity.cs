using Newtonsoft.Json;

namespace Domain
{
    public class BaseEntity
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
    }
}
