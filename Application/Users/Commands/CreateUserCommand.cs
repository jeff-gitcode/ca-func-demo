using Application.Abstraction;
using Domain;
using MediatR;

namespace Application.Users.Commands;

public record CreateUserCommand(User user) : IRequest<User> { }

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, User>
{
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<User> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        _userRepository.CreateUser(command.user);
        return Task.FromResult(command.user);
    }
}
