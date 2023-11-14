using Domain;

namespace Application.Abstraction;

public interface IUserRepository : IRepository<Customer>
{
    // User[] Search(Specification<User> specification, Pagination? pagination);
    // Task<User> CreateUser(User user);
    // User Get(int id);
    Task<Customer> GetByEmail(string email);
}
