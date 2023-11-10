using Application.Abstraction;
using Domain;

namespace Infrastructure;

public class UserRepository : IUserRepository
{
    public async Task<User> CreateUser(User user)
    {
        return user;
    }
}
