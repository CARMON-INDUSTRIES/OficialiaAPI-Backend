using Microsoft.EntityFrameworkCore;
using OficialiaCrudAPI.Data;
using OficialiaCrudAPI.DTO;
using OficialiaCrudAPI.Interfaces;

namespace OficialiaCrudAPI.Services
{
    public class AreaService : IAreaService
    {
        private readonly AppDataDbContext _context;
        public AreaService(AppDataDbContext context)
        {
            _context = context;
        }
        public async Task<List<AreaDto>> ObtenerAreas()
        {
            return await _context.Area.Select(x => new AreaDto
            {
                IdArea = x.IdArea,
                NombreArea = x.NombreArea
            }).ToListAsync();
        }
    }
}
