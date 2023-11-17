using Application.Users;
using AutoFixture.Xunit2;
using Domain;
using FluentAssertions;
using Infrastructure.CosmosDB;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using Moq;

namespace Infrastructure.Tests.CosmosDB
{
    public class UserRepositoryTests
    {
        private readonly Mock<Database> _database;
        private readonly Mock<Container> _container;
        private readonly Mock<IOptionsMonitor<CosmosDbOptions>> _cosmosDbOptions;
        private readonly Mock<CosmosClient> _cosmosClient;

        private readonly UserRepository _sut;

        public UserRepositoryTests() {
            _database = new Mock<Database>();
            _container = new Mock<Container>();
            _cosmosDbOptions = new Mock<IOptionsMonitor<CosmosDbOptions>>();
            _cosmosClient = new Mock<CosmosClient>();

            _sut = new UserRepository(_cosmosClient.Object, _cosmosDbOptions.Object);        
        }

        [Theory, AutoData]
        public async Task UserRepositoryTests_WhenAddItemAsync_ShouldReturns(
            Customer customer,
            ItemResponse<Customer> response
        )
        {
            _cosmosClient.Setup(r => r.GetDatabase(It.IsAny<string>())).Returns(_database.Object);
            
            _cosmosClient.Setup(r => r.GetContainer(It.IsAny<string>(), It.IsAny<string>())).Returns(_container.Object);

            _container.Setup(r => r.CreateItemAsync<Customer>(It.IsAny<Customer>(), It.IsAny<PartitionKey>(), null, default)).ReturnsAsync(response);

            var actual = await _sut.AddItemAsync(customer, default);

            actual.Should().BeEquivalentTo(customer);
        }

    }
}
