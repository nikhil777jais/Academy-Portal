using AutoMapper;
using AcademyPortalAPI.DTOs;
using AcademyPortalAPI.Models;
using AcademyPortalAPI.Extensions;
using AcademyPortalAPI.Repository.UnitOfWork;
#nullable disable
namespace AcademyPortalAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        private readonly IUnitOfWork _uow;

        public AutoMapperProfiles(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public AutoMapperProfiles()
        {
            CreateMap<ApplicationUser, ProfileDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.status.Name))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.UserRoles.FirstOrDefault().ApplicationRole.Name));
            
            CreateMap<ApplicationRole, RoleDto>();

            CreateMap<Status, StatusDto>();
        }
    }
}