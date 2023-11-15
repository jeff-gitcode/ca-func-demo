using Application.Abstraction;
using Application.Services;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Users.Queries;

public record LoginUserQuery(UserDto user) : IRequest<Customer> { }

public class LoginUserQueryHandler : BaseHandler, IQueryHandler<LoginUserQuery, Customer>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwtService;

    public LoginUserQueryHandler(
        IUserRepository userRepository,
        IMapper mapper,
        IJwtService jwtService
    )
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _jwtService = jwtService;
    }

    public async Task<Customer> Handle(LoginUserQuery query, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<Customer>(query.user);

        var selectedUser = await _userRepository.GetByEmail(query.user.Email);

        var tokens = ClaimBuilder
            .Create()
            .SetEmail(user.Email)
            .SetRole(user.Role)
            .SetId(user.Id.ToString())
            .Build();

        var token = _jwtService.BuildToken(tokens);

        var newUesr = new Customer()
        {
            Id = selectedUser.Id,
            Name = selectedUser.Name,
            Email = selectedUser.Email,
            Password = selectedUser.Password,
            Token = token
        };

        return await Response(newUesr);
    }
}
