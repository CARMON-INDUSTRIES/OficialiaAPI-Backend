using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using OficialiaCrudAPI.Models;
﻿using Microsoft.AspNetCore.Authorization;
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
    private readonly AppDataDbContext _context;

    public RolesController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, AppDataDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPut("usuarios/{id}")]
    public async Task<IActionResult> ActualizarUsuario(string id, ActualizarUsuarioRequest request)
    {
        var nombre = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var editor = string.IsNullOrEmpty(nombre) ? null : await _userManager.FindByNameAsync(nombre);
        if (editor == null || !await _userManager.IsInRoleAsync(editor, "SuperAdmin"))
            return Forbid(JwtBearerDefaults.AuthenticationScheme);

        var usuario = await _userManager.FindByIdAsync(id);
        if (usuario == null)
            return NotFound(new { mensaje = "Usuario no encontrado." });

        var rol = request.Rol == null ? null : await _roleManager.FindByNameAsync(request.Rol);
        if (request.Rol != null && rol?.Name == null)
            return BadRequest(new { mensaje = "El rol no existe." });

        if (rol?.Name != null && editor.Id == usuario.Id && rol.Name != "SuperAdmin")
            return BadRequest(new { mensaje = "No puedes quitar tu propio permiso de SuperAdmin." });

        var area = request.AreaId.HasValue ? await _context.Area.FindAsync(request.AreaId.Value) : null;
        if (request.AreaId.HasValue && area == null)
            return BadRequest(new { mensaje = "El área no existe." });

        await using var transaction = await _context.Database.BeginTransactionAsync();

        if (request.Correo != null && request.Correo != usuario.Email)
        {
            var resultado = await _userManager.SetEmailAsync(usuario, request.Correo);
            if (!resultado.Succeeded)
                return BadRequest(new { mensaje = "No se pudo actualizar el correo." });
        }

        if (rol?.Name is string rolNuevo)
        {
            var rolesActuales = await _userManager.GetRolesAsync(usuario);
            var rolesAnteriores = rolesActuales.Where(r => r != rolNuevo).ToArray();
            if (rolesAnteriores.Length > 0 &&
                !(await _userManager.RemoveFromRolesAsync(usuario, rolesAnteriores)).Succeeded)
                return BadRequest(new { mensaje = "No se pudo actualizar el rol." });

            if (!rolesActuales.Contains(rolNuevo) &&
                !(await _userManager.AddToRoleAsync(usuario, rolNuevo)).Succeeded)
                return BadRequest(new { mensaje = "No se pudo asignar el rol." });
        }

        if (area != null)
        {
            var asignaciones = await _context.UsuarioArea.Where(a => a.UserId == id)
                .OrderBy(a => a.Id).ToListAsync();
            if (asignaciones.Count == 0)
            {
                _context.UsuarioArea.Add(new UsuarioArea { UserId = id, AreaId = area.IdArea });
            }
            else
            {
                asignaciones[0].AreaId = area.IdArea;
                _context.UsuarioArea.RemoveRange(asignaciones.Skip(1));
            }

            await _context.SaveChangesAsync();
        }

        await transaction.CommitAsync();
        return NoContent();
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
public class ActualizarUsuarioRequest
{
    [EmailAddress]
    [StringLength(256)]
    public string? Correo { get; set; }
    public int? AreaId { get; set; }
    public string? Rol { get; set; }
}
