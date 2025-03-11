using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OficialiaCrudAPI.Interfaces;
using OficialiaCrudAPI.Services;
using System;
using System.Threading.Tasks;

namespace OficialiaCrudAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CorrespondenciaController : ControllerBase
    {
        private readonly ICorrespondenciaService _service;
        private readonly IComunidadesService _comunidadesService;
        private readonly IAreaService _areaService;
        private readonly IImportanciaService _importanciaService;
        private readonly IStatusService _statusService;
        private readonly UserManager<IdentityUser> _userManager;


        public CorrespondenciaController(ICorrespondenciaService service, IComunidadesService comunidadesService
            , IAreaService areaService, IImportanciaService importanciaService, IStatusService statusService, UserManager<IdentityUser> userManager)
        {
            _service = service;
            _comunidadesService = comunidadesService;
            _areaService = areaService;
            _importanciaService = importanciaService;
            _statusService = statusService;
            _userManager = userManager;

        }

        [HttpOptions("registrar")]
        public IActionResult RegistrarOptions()
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "https://oficialia-frontend-login.vercel.app");
            Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
            Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
            return Ok();
        }

        [HttpGet("obtener")]
        public async Task<IActionResult> ObtenerCorrespondencias(string userId)
        {
            var usuario = await _userManager.FindByIdAsync(userId);
            if (usuario == null)
            {
                return NotFound(new { mensaje = "Usuario no encontrado." });
            }

            // Verifica si el usuario es SuperAdmin
            var roles = await _userManager.GetRolesAsync(usuario);
            bool esSuperAdmin = roles.Contains("SuperAdmin");

            var datos = esSuperAdmin
                ? await _service.ObtenerTodasLasCorrespondencias() // Obtiene todo si es SuperAdmin
                : await _service.ObtenerCorrespondencias(userId); // Obtiene solo las del área del usuario

            return Ok(datos);
        }


        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarCorrespondencia([FromBody] CorrespondenciaDto correspondenciaDto)
        {
            try
            {
                if (correspondenciaDto == null)
                {
                    return BadRequest("Los datos de la correspondencia son inválidos.");
                }

                var resultado = await _service.RegistrarCorrespondencia(correspondenciaDto);

                if (!resultado)
                {
                    return StatusCode(500, "Ocurrió un error al registrar la correspondencia.");
                }

                return Ok(new { mensaje = "Correspondencia registrada exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }

        [HttpGet("obtener-comunidades")]
        public async Task<IActionResult> ObtenerComunidades()
        {
            if (_comunidadesService == null)
            {
                return StatusCode(500, "El servicio de comunidades no está inicializado.");
            }

            var comunidades = await _comunidadesService.ObtenerComunidades();
            return Ok(comunidades);
        }

        [HttpGet("obtener-areas")]
        public async Task<IActionResult> ObtenerAreas()
        {
            if (_areaService == null)
            {
                return StatusCode(500, "El servicio de áreas no está inicializado.");
            }

            var area = await _areaService.ObtenerAreas();
            return Ok(area);
        }

        [HttpGet("obtener-importancia")]
        public async Task<IActionResult> ObtenerImportancias()
        {
            if (_importanciaService == null)
            {
                return StatusCode(500, "El servicio de importancia no está inicializado.");
            }

            var area = await _importanciaService.ObtenerImportancias();
            return Ok(area);
        }

        [HttpGet("obtener-status")]
        public async Task<IActionResult> ObtenerStatus()
        {
            if (_statusService == null)
            {
                return StatusCode(500, "El servicio de status no está inicializado.");
            }

            var area = await _statusService.ObtenerStatus();
            return Ok(area);
        }

        [HttpDelete("eliminar/{id}")]
        public async Task<IActionResult> EliminarCorrespondencia(int id)
        {
            var resultado = await _service.EliminarCorrespondencia(id);

            if (!resultado)
            {
                return NotFound(new { mensaje = "No se encontró la correspondencia con el folio proporcionado." });
            }

            return Ok(new { mensaje = "Correspondencia eliminada exitosamente." });
        }

        [HttpPut("editar/{id}")]
        public async Task<IActionResult> EditarCorrespondencia(int id, [FromBody] CorrespondenciaDto correspondenciaDto)
        {
            if (correspondenciaDto == null || id != correspondenciaDto.Id)
            {
                return BadRequest("Datos inválidos.");
            }

            var resultado = await _service.EditarCorrespondencia(correspondenciaDto);

            if (!resultado)
            {
                return NotFound("No se encontró la correspondencia para actualizar.");
            }

            return Ok(new { mensaje = "Correspondencia actualizada exitosamente" });
        }

        
        [HttpGet("nuevasCorrespondencias/{ultimaFecha}")]
        public async Task<IActionResult> ObtenerNuevasCorrespondencias(DateTime ultimaFecha)
        {
            var nuevasCorrespondencias = await _service.ObtenerNuevasCorrespondencias(ultimaFecha);
            return Ok(new { nuevasCorrespondencias });
        }
    }
}
