using AutoFixture;
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
        private readonly Mock<Container> _container;
        private readonly Mock<IOptionsMonitor<CosmosDbOptions>> _cosmosDbOptions;
        private readonly Mock<CosmosClient> _cosmosClient;

        private readonly UserRepository _sut;

        public UserRepositoryTests()
        {
            var fixture = new Fixture();
            var dbOptions = fixture.Create<CosmosDbOptions>();
            _container = new Mock<Container>();
            _cosmosDbOptions = new Mock<IOptionsMonitor<CosmosDbOptions>>();
            _cosmosDbOptions.Setup(r => r.CurrentValue).Returns(dbOptions);
            _cosmosClient = new Mock<CosmosClient>();

            _cosmosClient
                .Setup(r => r.GetDatabase(It.IsAny<string>()).GetContainer(It.IsAny<string>()))
                .Returns(_container.Object);

            _sut = new UserRepository(_cosmosClient.Object, _cosmosDbOptions.Object);
        }

        [Theory, AutoData]
        public async Task UserRepositoryTests_WhenAddItemAsync_ShouldReturns(Customer customer)
        {
            Mock<ItemResponse<Customer>> itemResponse = new Mock<ItemResponse<Customer>>();
            itemResponse.Setup(_ => _.Resource).Returns(customer);

            _container
                .Setup(
                    r =>
                        r.CreateItemAsync<Customer>(
                            It.IsAny<Customer>(),
                            It.IsAny<PartitionKey>(),
                            It.IsAny<ItemRequestOptions>(),
                            It.IsAny<CancellationToken>()
                        )
                )
                .ReturnsAsync(itemResponse.Object);

            var actual = await _sut.AddItemAsync(customer, default);

            actual.Should().BeEquivalentTo(customer);
        }
    }
}
