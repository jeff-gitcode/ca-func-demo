using Application.Services;
using Application.Users;
using Domain;
using System.Linq.Expressions;

namespace Application.Services
{
    public abstract class Specification<T>
        where T : BaseEntity
    {
        public abstract Expression<Func<T, bool>> Build();
    }
}

public abstract class BaseSpecification<T, TSearchDto> : Specification<T>
    where T : BaseEntity
    where TSearchDto : class
{
    public TSearchDto? SearchDto { get; set; }

    public BaseSpecification(TSearchDto? searchDto) => SearchDto = searchDto;

    public bool IsNull() => SearchDto == null;
}

public class UserSpecification : BaseSpecification<Customer, SearchUserDto>
{
    public UserSpecification(SearchUserDto searchDto)
        : base(searchDto) { }

    public override Expression<Func<Customer, bool>> Build() =>
        e => IsNull() || e.Name.Contains(SearchDto!.Name!);
}
