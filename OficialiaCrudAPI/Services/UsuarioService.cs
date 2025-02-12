using Microsoft.EntityFrameworkCore;
using OficialiaCrudAPI.Data;
using OficialiaCrudAPI.DTO;

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
    }
}
