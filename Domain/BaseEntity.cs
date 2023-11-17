using Newtonsoft.Json;

namespace Domain
{
    public record BaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }
}
