using AutoMapper;
using AcademyPortalAPI.DTOs;
using AcademyPortalAPI.Models;
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

            CreateMap<ApplicationUser, FacultyDto>();
            CreateMap<ApplicationRole, RoleDto>();

            CreateMap<Status, StatusDto>();

            CreateMap<Skill, SkillNameDto>();

            CreateMap<Skill, SkillDto>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy.Email));

            CreateMap<Module, ModuleNameDto>();

            CreateMap<Module, ModuleDto>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy.Email));

            CreateMap<Skill, ModuleSkillDto>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy.Email))
                .ForMember(dest => dest.RelatedModules, opt => opt.MapFrom(src => src.RelatedModules));

            CreateMap<BatchUser, BatchUserStatusDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.User.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.status.Name));

            CreateMap<BatchDto, Batch>();

            CreateMap<Batch, BatchDto>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy.Email));
            
            CreateMap<Batch, DetailedBatchDto>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy.Email));

        }
    }
}