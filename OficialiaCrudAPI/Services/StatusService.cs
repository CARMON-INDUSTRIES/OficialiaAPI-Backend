using Microsoft.EntityFrameworkCore;
using OficialiaCrudAPI.Data;
using OficialiaCrudAPI.DTO;

namespace OficialiaCrudAPI.Services
{
    public class StatusService : IStatusService
    {
        private readonly AppDataDbContext _context;
        public StatusService(AppDataDbContext context)
        {
            _context = context;
        }
        public async Task<List<StatusDto>> ObtenerStatus()
        {
            var status = await _context.Status.ToListAsync();
            return status.Select(s => new StatusDto
            {
                IdStatus = s.IdStatus,
                Estado = s.Estado
            }).ToList();
        }
    }
    
}
