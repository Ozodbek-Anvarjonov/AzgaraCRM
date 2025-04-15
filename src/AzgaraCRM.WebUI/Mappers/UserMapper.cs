using AutoMapper;
using AzgaraCRM.WebUI.Domain.Entities;
using AzgaraCRM.WebUI.Models.Users;

namespace AzgaraCRM.WebUI.Mappers;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<CreateUserModel, User>();
        CreateMap<UpdateUserModel, User>();
        CreateMap<User, UserModelView>();
    }
}