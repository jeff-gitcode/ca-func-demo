using Application.Abstraction;
using Application.Users;
using Application.Users.Commands;
using Application.Users.Queries;
using AutoFixture.Xunit2;
using AutoMapper;
using Domain;
using FluentAssertions;
using Moq;
using System.Security.Claims;

namespace Application.Tests.Users
{
    public class LoginUserQueryHandlerTest
    {
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IJwtService> _jwtService;

        private readonly LoginUserQueryHandler _sut;

        public LoginUserQueryHandlerTest()
        {
            _mapper = new Mock<IMapper>();
            _userRepository = new Mock<IUserRepository>();
            _jwtService = new Mock<IJwtService>();

            _sut = new LoginUserQueryHandler(
                _userRepository.Object,
                _mapper.Object,
                _jwtService.Object
            );
        }

        [Theory, AutoData]
        public async Task LoginUserQueryHandlerTest_WhenHandle_ShouldReturns(
            Customer customer,
            UserDto userDto
        )
        {
            _userRepository.Setup(r => r.GetByEmail(It.IsAny<string>())).ReturnsAsync(customer);

            _mapper.Setup(r => r.Map<Customer>(It.IsAny<UserDto>())).Returns(customer);

            _jwtService.Setup(r => r.BuildToken(It.IsAny<IEnumerable<Claim>>())).Returns("token");

            var query = new LoginUserQuery(userDto);

            var actual = await _sut.Handle(query, default);

            actual.Should().BeEquivalentTo(customer with { Token = "token" });
        }
    }
}
