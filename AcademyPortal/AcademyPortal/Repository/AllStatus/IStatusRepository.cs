using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AcademyPortal.Models;
namespace AcademyPortal.Repository.AllStatus
{
    public interface IStatusRepository
    {
        Task<Status> GetStatusByIdAsync(int id);
        Task<Status> GetStatusByNameAsync(string name);
        Task<IEnumerable<Status>> GetAllStatus();        
    }
}