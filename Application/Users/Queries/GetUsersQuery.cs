using Application.Abstraction;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Users.Queries
{
    public record GetUsersQuery(SearchUserDto search) : IRequest<List<Customer>> { }

    public class GetUsersQueryHandler : BaseHandler, IQueryHandler<GetUsersQuery, List<Customer>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<List<Customer>> Handle(
            GetUsersQuery request,
            CancellationToken cancellationToken
        )
        {
            var dto = request.search;

            var (page, rows) = dto;

            Pagination pagination = new(page, rows);
            
            UserSpecification specification = new(dto);

            List<Customer> users = await _userRepository.Search(specification, pagination);

            return await Response(users);
        }
    }

    public static class LinqHelpers
    {
        public static async Task<List<TSource>> ToListAsync<TSource>(
            this IAsyncEnumerable<TSource> source,
            CancellationToken cancellationToken = default
        )
        {
            var list = new List<TSource>();

            await foreach (var element in source.WithCancellation(cancellationToken))
            {
                list.Add(element);
            }

            return list;
        }
    }
}
