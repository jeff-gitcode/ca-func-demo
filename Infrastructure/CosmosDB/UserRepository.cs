using Application.Abstraction;
using Application.Services;
using Ardalis.GuardClauses;
using Domain;
using Infrastructure.Jwt;
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
            Guard.Against.Null(cosmosDbOptions.CurrentValue, nameof(cosmosDbOptions));
            Guard.Against.Null(dbClient, nameof(dbClient));

            _cosmosDbOptions = cosmosDbOptions.CurrentValue;
            _container = dbClient
                .GetDatabase(_cosmosDbOptions.Database)
                .GetContainer(_cosmosDbOptions.Containers[0].Name);
        }

        public async Task<Customer> AddItemAsync(Customer user, CancellationToken cancellationToken)
        {
            var response = await _container.CreateItemAsync<Customer>(
                user,
                new PartitionKey(user.Email)
            );
            return response;
        }

        public async IAsyncEnumerable<Customer> GetAll()
        {
            var queryDefinition = new QueryDefinition(@"SELECT * FROM Customer");

            var iterator = _container.GetItemQueryIterator<Customer>(queryDefinition, null);

            while (iterator.HasMoreResults)
                foreach (var item in await iterator.ReadNextAsync().ConfigureAwait(false))
                    yield return item;
        }

        public async Task<Customer> GetByEmail(string email)
        {
            var queryDefinition = new QueryDefinition(
                "SELECT * FROM Customer C WHERE C.Email = @email"
            ).WithParameter("@email", email);

            var iterator = _container.GetItemQueryIterator<Customer>(queryDefinition, null);

            while (iterator.HasMoreResults)
            {
                var documents = await iterator.ReadNextAsync();
                return documents.FirstOrDefault();
            }

            return null;
        }

        public async Task<List<Customer>> Search(
            Specification<Customer> specification,
            Pagination? pagination
        )
        {
            var results = await GetAll().ToIQueryable();

            return results.Filter(specification).Paginate(pagination).ToList();
        }
    }

    public static class LinqHelpers
    {
        public static async Task<IQueryable<TSource>> ToIQueryable<TSource>(
            this IAsyncEnumerable<TSource> source,
            CancellationToken cancellationToken = default
        )
        {
            var list = new List<TSource>();

            await foreach (var element in source.WithCancellation(cancellationToken))
            {
                list.Add(element);
            }

            return list.AsQueryable();
        }
    }
}
