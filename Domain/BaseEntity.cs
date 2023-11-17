using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Domain
{
    public record BaseEntity
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }
}
