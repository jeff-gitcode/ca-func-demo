using Application.Abstraction;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Users.Commands;

public record RegisterUserCommand(UserDto user) : IRequest<User> { }

public class RegisterUserCommandHandler : BaseHandler, ICommandHandler<RegisterUserCommand, User>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public RegisterUserCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<User> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
            var user = _mapper.Map<User>(command.user);

            var response = await _userRepository.CreateUser(user);

            return await Response(response);
    }
}
