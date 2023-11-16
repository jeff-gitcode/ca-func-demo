using Application.Abstraction;
using AutoMapper;
using Domain;
using FluentValidation;
using System.Text.Json.Serialization;

namespace Application.Users
{
    public class UserDto : BaseEntity, IValidateable<UserDto>
    {
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        [JsonIgnore]
        public AbstractValidator<UserDto> Validator => new UserDtoValidation();
    }

    public class UserDtoValidation : AbstractValidator<UserDto>
    {
        public UserDtoValidation()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.Role).NotEmpty();
        }
    }

    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<Customer, UserDto>().ReverseMap();
        }
    }

    public class UserDtoMapper : Profile
    {
        public UserDtoMapper()
        {
            CreateMap<UserDto, Customer>().ReverseMap();
        }
    }
}
