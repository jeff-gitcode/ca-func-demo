using Application.Abstraction;
using AutoMapper;
using Domain;
using MediatR;
using Newtonsoft.Json;

namespace Application.Users.Commands;

public record CreateUserCommand(UserDto user) : IRequest<string> { }

public class CreateUserCommandHandler : BaseHandler, ICommandHandler<CreateUserCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<string> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var user = _mapper.Map<User>(command.user);

            var response = await _userRepository.CreateUser(user);

            return await Success(JsonConvert.SerializeObject(response));

        }
        catch (Exception ex)
        {

            throw;
        }
    }
}
