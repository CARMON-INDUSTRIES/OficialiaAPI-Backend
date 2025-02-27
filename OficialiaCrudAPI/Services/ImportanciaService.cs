using Microsoft.EntityFrameworkCore;
using OficialiaCrudAPI.Data;
using OficialiaCrudAPI.DTO;
using OficialiaCrudAPI.Interfaces;

namespace OficialiaCrudAPI.Services
{
    public class ImportanciaService : IImportanciaService
    {
        private readonly AppDataDbContext _context;

        public ImportanciaService(AppDataDbContext context)
        {
            _context = context;
        }

        public async Task<List<ImportanciaDto>> ObtenerImportancias()
        {
            return await _context.Importancia
                .Select(c => new ImportanciaDto
                {
                    IdImportancia = c.IdImportancia,
                    Nivel = c.Nivel
                })
                .ToListAsync();
        }
    }
}
