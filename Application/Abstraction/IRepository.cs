using Domain;

namespace Application.Abstraction
{
    public interface IRepository<TEntity>
        where TEntity : BaseEntity
    {
        Task<TEntity> AddItemAsync(Customer user, CancellationToken cancellationToken);
    }
}
