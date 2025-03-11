using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OficialiaCrudAPI.Data;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public RolesController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpPost("AsignarRol")]
    public async Task<IActionResult> AsignarRol([FromBody] AsignarRolRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);
        if (user == null)
        {
            return NotFound(new { mensaje = "Usuario no encontrado." });
        }

        if (!await _roleManager.RoleExistsAsync(request.Role))
        {
            return BadRequest(new { mensaje = $"El rol '{request.Role}' no existe en la base de datos." });
        }

        if (await _userManager.IsInRoleAsync(user, request.Role))
        {
            return BadRequest(new { mensaje = $"El usuario ya tiene el rol '{request.Role}'." });
        }

        var result = await _userManager.AddToRoleAsync(user, request.Role);
        if (result.Succeeded)
        {
            return Ok(new { mensaje = $"Rol '{request.Role}' asignado a '{request.UserName}' correctamente." });
        }

        return BadRequest(new { mensaje = "Error al asignar el rol.", errores = result.Errors });
    }

    [HttpPost("QuitarRol")]
    public async Task<IActionResult> QuitarRol([FromBody] QuitarRolRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);
        if (user == null)
        {
            return NotFound(new { mensaje = "Usuario no encontrado." });
        }

        if (!await _userManager.IsInRoleAsync(user, request.Role))
        {
            return BadRequest(new { mensaje = $"El usuario no tiene el rol '{request.Role}'." });
        }

        var result = await _userManager.RemoveFromRoleAsync(user, request.Role);
        if (result.Succeeded)
        {
            return Ok(new { mensaje = $"Rol '{request.Role}' eliminado de '{request.UserName}' correctamente." });
        }

        return BadRequest(new { mensaje = "Error al quitar el rol.", errores = result.Errors });
    }


    [HttpGet("GetRoles")]
    public IActionResult GetRoles()
    {
        var roles = _roleManager.Roles.Select(r => new { r.Id, r.Name }).ToList();
        return Ok(roles);
    }

    [HttpGet("GetUsersWithRoles")]
    public async Task<IActionResult> GetUsersWithRoles()
    {
        var users = _userManager.Users.ToList();
        var usersWithRoles = new List<object>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            usersWithRoles.Add(new
            {
                user.Id,
                user.UserName, 
                user.Email,
                Roles = roles
            });
        }

        return Ok(usersWithRoles);
    }
}

public class AsignarRolRequest
{
    public string UserName { get; set; }
    public string Role { get; set; }
}


public class QuitarRolRequest
{
    public string UserName { get; set; }
    public string Role { get; set; }
}