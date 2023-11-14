namespace Infrastructure.CosmosDB
{
    public class CosmosDbOptions
    {
        public const string CosmosDb = "CosmosDb";
        public required string EndpointUrl { get; set; }
        public required string PrimaryConnectionString { get; set; }
        public required string Database { get; set; }

        public required ContainerInfo[] Containers { get; set; }
    }

    public class ContainerInfo
    {
        public string Name { get; set; } = string.Empty;
        public string PartitionKey { get; set; } = string.Empty;
    }
}
