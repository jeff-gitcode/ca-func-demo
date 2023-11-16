using Newtonsoft.Json;

namespace Domain
{
    public class BaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }
}
