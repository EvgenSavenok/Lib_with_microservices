using AutoMapper;
using Entities;
using Entities.DataTransferObjects;

namespace Library_Web_Application;

public class Mapping : Profile
{
    public Mapping()
    {
        CreateMap<UserForRegistrationDto, User>();
    }
}

