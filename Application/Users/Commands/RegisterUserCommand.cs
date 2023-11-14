using Application.Abstraction;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Users.Commands;

public record RegisterUserCommand(UserDto user) : IRequest<Customer> { }

public class RegisterUserCommandHandler
    : BaseHandler,
        ICommandHandler<RegisterUserCommand, Customer>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public RegisterUserCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<Customer> Handle(
        RegisterUserCommand command,
        CancellationToken cancellationToken
    )
    {
        var user = _mapper.Map<Customer>(command.user);

        var response = await _userRepository.AddItemAsync(user, default);

        return await Response(response);
    }
}
