using Application.Abstraction;
using Application.Users;
using Application.Users.Commands;
using Application.Users.Queries;
using AutoFixture.Xunit2;
using AutoMapper;
using Domain;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tests.Users
{
    public class SearchUsersQueryHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IMapper> _mapper;

        private readonly SearchUsersQueryHandler _sut;

        public SearchUsersQueryHandlerTests()
        {
            _mapper = new Mock<IMapper>();
            _userRepository = new Mock<IUserRepository>();

            _sut = new SearchUsersQueryHandler(_userRepository.Object, _mapper.Object);
        }

        [Theory, AutoData]
        public async Task SearchUsersQueryHandlerTests_WhenHandle_ShouldReturns(
            List<Customer> customers,
            SearchUserDto searchUserDto,
            UserSpecification userSpecification
        )
        {
            _userRepository
                .Setup(r => r.Search(It.IsAny<UserSpecification>(), It.IsAny<Pagination>()))
                .ReturnsAsync(customers);

            var query = new SearchUsersQuery(searchUserDto);

            var actual = await _sut.Handle(query, default);

            actual.Should().BeEquivalentTo(customers);
        }
    }
}
