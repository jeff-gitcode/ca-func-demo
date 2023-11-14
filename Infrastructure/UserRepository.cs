//using Application.Abstraction;
//using Domain;

//namespace Infrastructure;

//public class UserRepository : BaseRepository<Customer>, IUserRepository
//{
//    //public async Task<User> CreateUser(User user)
//    //{
//    //    var newUser = new User()
//    //    {
//    //        Id = user.Id,
//    //        Name = user.Name,
//    //        Role = user.Role,
//    //        Email = user.Email,
//    //        Password = user.Password,
//    //    };

//    //    return newUser;
//    //}
//    public UserRepository(Microsoft.Azure.Cosmos.CosmosClient cosmosClient, string databaseName, string containerName) : base(cosmosClient, databaseName, containerName)
//    {

//    }

//    public override bool Equals(object? obj)
//    {
//        return base.Equals(obj);
//    }

//    public async Task<Customer> GetByEmail(string email)
//    {
//        return new Customer()
//        {
//            Email = email,
//        };
//    }

//    public override int GetHashCode()
//    {
//        return base.GetHashCode();
//    }

//    public override string? ToString()
//    {
//        return base.ToString();
//    }
//}
