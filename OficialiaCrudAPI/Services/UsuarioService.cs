using Microsoft.EntityFrameworkCore;
using OficialiaCrudAPI.Data;
using OficialiaCrudAPI.DTO;
using OficialiaCrudAPI.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OficialiaCrudAPI.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly AppDataDbContext _context;

        public UsuarioService(AppDataDbContext context)
        {
            _context = context;
        }

        public async Task<List<UsuarioDto>> ObtenerUsuarios()
        {
            return await _context.Users.Select(x => new UsuarioDto
            {
                Id = x.Id,
                Name = x.UserName
            }).ToListAsync();
        }

        public async Task<UsuarioAreaDto> ObtenerUsuarioArea(string userId) 
        {
            var usuarioArea = await _context.UsuarioArea
                .Where(ua => ua.UserId == userId)
                .Select(ua => new UsuarioAreaDto
                {
                    UserId = ua.UserId,
                    AreaId = ua.AreaId
                })
                .FirstOrDefaultAsync();

            return usuarioArea;
        }

        public async Task EliminarUsuarioAreaPorUsuarioId(string userId)
        {
            var registrosRelacionados = await _context.UsuarioArea
                .Where(x => x.UserId == userId)
                .ToListAsync();

            if (registrosRelacionados.Any())
            {
                _context.UsuarioArea.RemoveRange(registrosRelacionados);
                await _context.SaveChangesAsync();
            }
        }
    }
}
