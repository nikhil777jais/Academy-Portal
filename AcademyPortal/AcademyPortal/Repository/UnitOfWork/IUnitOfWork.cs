using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyPortal.Repository.User;

namespace AcademyPortal.Repository.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository {get;}
        Task<bool> SaveChanges();
    }
}