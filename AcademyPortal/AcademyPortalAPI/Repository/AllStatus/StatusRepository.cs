using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AcademyPortalAPI.Models;
using AcademyPortalAPI.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace AcademyPortalAPI.Repository.AllStatus
{
    public class StatusRepository : IStatusRepository
    {
        private readonly AcademyPortalDbContext _db;
        private readonly IMapper _mapper;

        public StatusRepository(AcademyPortalDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Status>> GetAllStatus()
        {
            return await _db.AllStatus.ToListAsync();
        }

        public async Task<IEnumerable<StatusDto>> GetAllStatusDtos()
        {
            return await _db.AllStatus.ProjectTo<StatusDto>(_mapper.ConfigurationProvider).ToListAsync();
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