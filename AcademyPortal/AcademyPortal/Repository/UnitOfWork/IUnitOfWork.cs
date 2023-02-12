using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyPortal.Repository.Roles;
using AcademyPortal.Repository.User;
using AcademyPortal.Repository.AllStatus;
using AcademyPortal.Repository.Skills;
using AcademyPortal.Repository.Modules;
using AcademyPortal.Repository.Batches;
using AcademyPortal.Repository.BatchUsers;

namespace AcademyPortal.Repository.UnitOfWork
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
        Task<bool> SaveChangesAsync();
    }
}