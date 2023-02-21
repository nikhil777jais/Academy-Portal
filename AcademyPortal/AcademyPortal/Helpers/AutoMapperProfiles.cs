using AutoMapper;
using AcademyPortal.DTOs;
using AcademyPortal.Models;
using AcademyPortal.Extensions;
using AcademyPortal.Repository.UnitOfWork;
#nullable disable
namespace AcademyPortal.Helpers
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
        }
    }
}
