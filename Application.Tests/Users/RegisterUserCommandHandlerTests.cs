using Application.Abstraction;
using Application.Users;
using Application.Users.Commands;
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
    public class RegisterUserCommandHandlerTests
    {
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IMapper> _mapper;

        private readonly RegisterUserCommandHandler _sut;

        public RegisterUserCommandHandlerTests()
        {
            _userRepository = new Mock<IUserRepository>();
            _mapper = new Mock<IMapper>();

            _sut = new RegisterUserCommandHandler(_userRepository.Object, _mapper.Object);
        }

        [Theory, AutoData]
        public async Task RegisterUserCommandTests_WhenHandle_ShouldReturns(
            Customer customer,
            UserDto userDto
        )
        {
            _userRepository.Setup(r => r.GetByEmail(It.IsAny<string>())).ReturnsAsync(customer);

            _mapper.Setup(r => r.Map<Customer>(It.IsAny<UserDto>())).Returns(customer);

            var command = new RegisterUserCommand(userDto);

            var actual = await _sut.Handle(command, default);

            actual.Should().BeEquivalentTo(customer);
        }
    }
}
