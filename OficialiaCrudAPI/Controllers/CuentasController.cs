using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using OficialiaCrudAPI.Models;
using System.Security.Claims;
using System.Text;
using OficialiaCrudAPI.Interfaces;

namespace OficialiaCrudAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class CuentasController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUsuarioService _service;
        private IConfiguration _config;


        public CuentasController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, 
            RoleManager<IdentityRole> roleManager, IUsuarioService service, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _service = service;
            _config = config;
        }

        [Route("UserNuevo")]
        [HttpPost]
        public async Task<IActionResult> Registration(UserRegistrationModel model)
        {
            var user = new IdentityUser { UserName = model.UserName, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("Admin"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                if (!await _roleManager.RoleExistsAsync("User"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("User"));
                }

                var anyUsers = _userManager.Users.Count();

                if (anyUsers == 1)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, "User");
                }

                return Ok(new { Status = "Success", Message = "Usuario registrado!", UserId = user.Id });
            }

            return BadRequest(new { Status = "Error", Message = "Error, fallo al registrar!", Errors = result.Errors });

        }


        [Route("UserLogin")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {

            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(model.UserName);
                var token = GenerateJwtToken(user);

                return Ok(new { token });
            }

            return Unauthorized();
        }

        [HttpDelete("EliminarUsuario/{id}")]
        public async Task<IActionResult> EliminarUsuario(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);

            if (usuario == null)
            {
                return NotFound(new { mensaje = "No se encontró el usuario con el ID proporcionado." });
            }

            try
            {
                // Eliminar registros relacionados en UsuarioArea
                await _service.EliminarUsuarioAreaPorUsuarioId(id);

                // Ahora eliminar el usuario
                var resultado = await _userManager.DeleteAsync(usuario);

                if (!resultado.Succeeded)
                {
                    return BadRequest(new { mensaje = "No se pudo eliminar el usuario.", errores = resultado.Errors });
                }

                return Ok(new { mensaje = "Usuario eliminado exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error interno.", error = ex.Message });
            }
        }


        private string GenerateJwtToken(IdentityUser user)
        {
            var roles = _userManager.GetRolesAsync(user).Result;
            var role = roles.FirstOrDefault();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.UserName),
                new Claim(ClaimTypes.Role,role)
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

    }
}
