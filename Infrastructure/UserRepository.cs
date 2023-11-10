using Application.Abstraction;
using Domain;

namespace Infrastructure;

public class UserRepository : IUserRepository
{
    public async Task<User> CreateUser(User user)
    {
        var newUser = new User()
        {
            Id = user.Id,
            Name = user.Name,
            Role = user.Role,
            Email = user.Email,
            Password = user.Password,
        };

        return newUser;
    }
}
