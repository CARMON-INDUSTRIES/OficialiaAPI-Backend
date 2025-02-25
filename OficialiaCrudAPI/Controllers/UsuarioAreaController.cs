using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OficialiaCrudAPI.Models;
using OficialiaCrudAPI.Data;

[Route("api/[controller]")]
[ApiController]
public class UsuarioAreaController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly AppDataDbContext _context;

    public UsuarioAreaController(UserManager<IdentityUser> userManager, AppDataDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    
    [HttpPost("AsignarArea")]
    public async Task<IActionResult> AsignarArea([FromBody] UsuarioAreaRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            return NotFound(new { mensaje = "Usuario no encontrado." });
        }

        var area = await _context.Area.FindAsync(request.AreaId);
        if (area == null)
        {
            return NotFound(new { mensaje = "Área no encontrada." });
        }

        var usuarioArea = new UsuarioArea
        {
            UserId = request.UserId,
            AreaId = request.AreaId
        };

        _context.UsuarioArea.Add(usuarioArea);
        await _context.SaveChangesAsync();

        return Ok(new { mensaje = "Área asignada correctamente." });
    }

    
    [HttpGet("GetAreaByUser/{userId}")]
    public async Task<IActionResult> GetAreaByUser(string userId)
    {
        var area = await _context.UsuarioArea
            .Where(ua => ua.UserId == userId)
            .Join(_context.Area, ua => ua.AreaId, a => a.IdArea, (ua, a) => a.NombreArea)
            .FirstOrDefaultAsync();

        if (area == null)
        {
            return NotFound(new { mensaje = "Área no encontrada." });
        }

        return Ok(new { area });
    }

   
    [HttpGet("GetUsersByArea/{areaId}")]
    public async Task<IActionResult> GetUsersByArea(int areaId)
    {
        var users = await _context.UsuarioArea
            .Where(ua => ua.AreaId == areaId)
            .Join(_userManager.Users, ua => ua.UserId, u => u.Id, (ua, u) => new
            {
                u.Id,
                u.UserName,
                u.Email
            }).ToListAsync();

        return Ok(users);
    }
}
