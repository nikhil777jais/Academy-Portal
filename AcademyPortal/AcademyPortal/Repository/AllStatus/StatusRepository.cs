using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AcademyPortal.Model;
using AcademyPortal.Models;

namespace AcademyPortal.Repository.AllStatus
{
    public class StatusRepository : IStatusRepository
    {
        private readonly AcademyPortalDbContext _db;

        public StatusRepository(AcademyPortalDbContext  db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Status>> GetAllStatus()
        {
            return await _db.AllStatus.ToListAsync();
        }

        public async Task<Status> GetStatusByIdAsync(int id)
        {
            return await _db.AllStatus.SingleOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Status> GetStatusByNameAsync(string name)
        {
            return await _db.AllStatus.SingleOrDefaultAsync(s => s.Name == name);
        }
    }
}