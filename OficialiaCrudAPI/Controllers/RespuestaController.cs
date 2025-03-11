using Microsoft.AspNetCore.Mvc;
using OficialiaCrudAPI.Interfaces;
using OficialiaCrudAPI.DTO;
using System.Threading.Tasks;

namespace OficialiaCrudAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RespuestaController : ControllerBase
    {
        private readonly IRespuesta _respuestaService;

        public RespuestaController(IRespuesta respuestaService)
        {
            _respuestaService = respuestaService;
        }

        [HttpGet("obtener")]
        public async Task<IActionResult> ObtenerRespuestas()
        {
            var respuestas = await _respuestaService.ObtenerRespuesta();
            return Ok(respuestas);
        }

        [HttpGet("obtener/{id}")]
        public async Task<IActionResult> ObtenerRespuestaPorId(int id)
        {
            var respuesta = await _respuestaService.ObtenerRespuestaPorId(id);
            if (respuesta == null)
            {
                return NotFound(new { mensaje = "Respuesta no encontrada" });
            }
            return Ok(respuesta);
        }

        [HttpPost("responder")]
        public async Task<IActionResult> CrearRespuesta([FromBody] RespuestaDto respuestaDto)
        {
            var resultado = await _respuestaService.CrearRespuesta(respuestaDto);
            if (!resultado)
            {
                return BadRequest(new { mensaje = "No se encontró la correspondencia especificada." });
            }
            return Ok(new { mensaje = "Respuesta creada exitosamente." });
        }
    }
}
