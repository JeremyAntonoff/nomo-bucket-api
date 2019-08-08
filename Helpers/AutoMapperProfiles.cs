using AutoMapper;
using NomoBucket.API.Dtos;
using NomoBucket.API.Models;

namespace NomoBucket.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserListDto>();
            CreateMap<User, UserDetailsDto>()
            .ForMember(dest => dest.Age, opt => {
                opt.MapFrom(d => d.DateOfBirth.CalculateAge());
            });
            CreateMap<BucketListItem, BucketListItemDto>();
        }
    }
}