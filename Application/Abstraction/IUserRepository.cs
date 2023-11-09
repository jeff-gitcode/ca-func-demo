using Domain;

namespace Application.Abstraction;

public interface IUserRepository
{
    // User[] Search(Specification<User> specification, Pagination? pagination);
    Task<User> CreateUser(User user);
    // User Get(int id);
    // User GetByEmail(string email);
}
