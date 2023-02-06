using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyPortal.Repository.Roles;
using AcademyPortal.Repository.User;

namespace AcademyPortal.Repository.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository {get;}
        public IRoleRepository RoleRepository {get;}
        Task<bool> SaveChangesAsync();
    }
}