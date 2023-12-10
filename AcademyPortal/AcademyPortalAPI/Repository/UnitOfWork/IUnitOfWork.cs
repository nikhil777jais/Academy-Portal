using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyPortalAPI.Repository.Roles;
using AcademyPortalAPI.Repository.User;
using AcademyPortalAPI.Repository.AllStatus;
using AcademyPortalAPI.Repository.Skills;
using AcademyPortalAPI.Repository.Modules;
using AcademyPortalAPI.Repository.Batches;
using AcademyPortalAPI.Repository.BatchUsers;
using AcademyPortalAPI.Services.Token;

namespace AcademyPortalAPI.Repository.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository {get;}
        public IRoleRepository RoleRepository {get;}
        public IStatusRepository StatusRepository {get;}
        public ISkillRepository SkillRepository {get;}
        public IModuleRepository ModuleRepository {get;} 
        public IBatchRepository BatchRepository {get;} 
        public IBatchUserRepository BatchUserRepository {get;} 
        public ITokenService TokenService {get;} 
        Task<bool> SaveChangesAsync();
    }
}