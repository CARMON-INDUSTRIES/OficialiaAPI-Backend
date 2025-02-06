namespace OficialiaCrudAPI.Services;
using OficialiaCrudAPI.DTO;
using OficialiaCrudAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ComunidadesService : IComunidadesService
{
    private readonly AppDataDbContext _context;

    public ComunidadesService(AppDataDbContext context)
    {
        _context = context;
    }

    public async Task<List<ComunidadesDto>> ObtenerComunidades()
    {
        return await _context.Comunidades
            .Select(c => new ComunidadesDto
            {
                IdComunidad = c.IdComunidad,
                NombreComunidad = c.NombreComunidad
            })
            .ToListAsync();
    }
}
