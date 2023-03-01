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
        public AutoMapperProfiles()
        {
            CreateMap<ApplicationUser, ProfileDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.status.Name))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.UserRoles.FirstOrDefault().ApplicationRole.Name));
            
            CreateMap<ApplicationRole, RoleDto>();

            CreateMap<Status, StatusDto>();

            CreateMap<Skill, SkillDto>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy.Email));

            CreateMap<Module, ModuleDto>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy.Email));
        }
    }
}