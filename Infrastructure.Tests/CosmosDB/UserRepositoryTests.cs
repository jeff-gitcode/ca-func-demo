using Application.Services;
using Application.Users;
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

        [Theory, AutoData]
        public async Task UserRepositoryTests_WhenGetByEmail_ShouldReturns(
            IEnumerable<Customer> customers
        )
        {
            Mock<FeedResponse<Customer>> mockedFeedResponse = new Mock<FeedResponse<Customer>>();
            mockedFeedResponse.Setup(c => c.GetEnumerator()).Returns(customers.GetEnumerator);

            Mock<FeedIterator<Customer>> mockedFeedIterator = new Mock<FeedIterator<Customer>>();
            mockedFeedIterator.SetupSequence(c => c.HasMoreResults).Returns(true).Returns(false);
            mockedFeedIterator
                .Setup(c => c.ReadNextAsync(default))
                .ReturnsAsync(mockedFeedResponse.Object);

            _container
                .Setup(
                    r =>
                        r.GetItemQueryIterator<Customer>(
                            It.IsAny<QueryDefinition>(),
                            It.IsAny<string>(),
                            It.IsAny<QueryRequestOptions>()
                        )
                )
                .Returns(mockedFeedIterator.Object);

            var actual = await _sut.GetByEmail("test@test.com");

            actual.Should().BeEquivalentTo(customers.FirstOrDefault());
        }

        [Theory, AutoData]
        public async Task UserRepositoryTests_WhenSearch_ShouldReturns(
            IEnumerable<Customer> customers
        )
        {
            Mock<FeedResponse<Customer>> mockedFeedResponse = new Mock<FeedResponse<Customer>>();
            mockedFeedResponse.Setup(c => c.GetEnumerator()).Returns(customers.GetEnumerator);

            Mock<FeedIterator<Customer>> mockedFeedIterator = new Mock<FeedIterator<Customer>>();
            mockedFeedIterator.SetupSequence(c => c.HasMoreResults).Returns(true).Returns(false);
            mockedFeedIterator
                .Setup(c => c.ReadNextAsync(default))
                .ReturnsAsync(mockedFeedResponse.Object);

            _container
                .Setup(
                    r =>
                        r.GetItemQueryIterator<Customer>(
                            It.IsAny<QueryDefinition>(),
                            It.IsAny<string>(),
                            It.IsAny<QueryRequestOptions>()
                        )
                )
                .Returns(mockedFeedIterator.Object);

            UserSpecification specification = new UserSpecification(
                new SearchUserDto() { Name = customers.FirstOrDefault().Name, }
            );

            Pagination pagination = new Pagination() { Page = 1, Rows = 1 };

            // var actual = await _sut.GetAll().ToIQueryable();
            var actual = await _sut.Search(specification, pagination);

            actual.FirstOrDefault().Name.Should().Be(specification.SearchDto.Name);
        }
    }
}
