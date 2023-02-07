using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyPortal.Repository.Roles;
using AcademyPortal.Repository.User;
using AcademyPortal.Repository.AllStatus;

namespace AcademyPortal.Repository.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository {get;}
        public IRoleRepository RoleRepository {get;}
        public IStatusRepository StatusRepository {get;}
        Task<bool> SaveChangesAsync();
    }
}