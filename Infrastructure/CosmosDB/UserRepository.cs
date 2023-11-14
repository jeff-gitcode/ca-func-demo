using Application.Abstraction;
using Domain;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace Infrastructure.CosmosDB
{
    public class UserRepository : IUserRepository
    {
        private readonly Container _container;
        private readonly CosmosDbOptions _cosmosDbOptions;

        public UserRepository(
            CosmosClient dbClient,
            IOptionsMonitor<CosmosDbOptions> cosmosDbOptions
        )
        {
            _cosmosDbOptions = cosmosDbOptions.CurrentValue;
            _container = dbClient
                .GetDatabase(_cosmosDbOptions.Database)
                .GetContainer(_cosmosDbOptions.Containers[0].Name);
        }

        public async Task<Customer> AddItemAsync(Customer user, CancellationToken cancellationToken)
        {
            var response = await _container.CreateItemAsync<Customer>(
                user,
                new Microsoft.Azure.Cosmos.PartitionKey(user.Email)
            );
            return response;
        }

        public async Task<Customer> GetByEmail(string email)
        {
            var queryDefinition = new QueryDefinition("SELECT * FROM Customer C WHERE C.Email = @email")
              .WithParameter("@email", email);

            var iterator = _container.GetItemQueryIterator<Customer>(queryDefinition, null);

            while(iterator.HasMoreResults)
            {
                var documents = await iterator.ReadNextAsync();
                return documents.FirstOrDefault();
            }

            return null;
        }
    }
}
