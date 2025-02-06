using Microsoft.AspNetCore.Mvc;
using OficialiaCrudAPI.DTO;
using OficialiaCrudAPI.Services;

namespace OficialiaCrudAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CorrespondenciaController : ControllerBase
    {
        private readonly ICorrespondenciaService _service;

        public CorrespondenciaController(ICorrespondenciaService service)
        {
            _service = service;
        }

        [HttpGet("obtener")]
        public async Task<IActionResult> ObtenerCorrespondencias()
        {
            var datos = await _service.ObtenerCorrespondencias();
            return Ok(datos);
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarCorrespondencia([FromBody] CorrespondenciaDto correspondenciaDto)
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
    }
}
