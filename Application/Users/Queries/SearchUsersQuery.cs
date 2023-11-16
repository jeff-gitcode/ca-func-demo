using Application.Abstraction;
using Ardalis.GuardClauses;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Users.Queries
{
    public record SearchUsersQuery(SearchUserDto search) : IRequest<List<Customer>> { }

    public class SearchUsersQueryHandler
        : BaseHandler,
            IQueryHandler<SearchUsersQuery, List<Customer>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public SearchUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            Guard.Against.Null(userRepository, nameof(userRepository));
            Guard.Against.Null(mapper, nameof(mapper));

            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<Customer>> Handle(
            SearchUsersQuery query,
            CancellationToken cancellationToken
        )
        {
            var dto = query.search;

            var (page, rows) = dto;

            Pagination pagination = new(page, rows);

            UserSpecification specification = new(dto);

            List<Customer> users = await _userRepository.Search(specification, pagination);

            return await Response(users);
        }
    }
}
