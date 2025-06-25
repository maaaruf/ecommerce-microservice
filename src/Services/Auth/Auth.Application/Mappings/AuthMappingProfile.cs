using AutoMapper;
using Auth.Domain.Entities;
using Shared.Contracts.Models;

namespace Auth.Application.Mappings;

public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => new List<string>()));
    }
} 