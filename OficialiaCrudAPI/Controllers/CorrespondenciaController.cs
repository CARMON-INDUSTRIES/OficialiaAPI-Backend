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
        private readonly IComunidadesService _comunidadesService;
        private readonly IAreaService _areaService;
        private readonly IImportanciaService _importanciaService;
        private readonly IStatusService _statusService;

        public CorrespondenciaController(ICorrespondenciaService service, IComunidadesService comunidadesService
            , IAreaService areaService, IImportanciaService importanciaService, IStatusService statusService)
        {
            _service = service;
            _comunidadesService = comunidadesService;
            _areaService = areaService;
            _importanciaService = importanciaService;
            _statusService = statusService;
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
                return StatusCode(500, "El servicio de areas no está inicializado.");
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

        [HttpDelete("eliminar/{folio}")]
        public async Task<IActionResult> EliminarCorrespondencia(int folio)
        {
            var resultado = await _service.EliminarCorrespondencia(folio);

            if (!resultado)
            {
                return NotFound(new { mensaje = "No se encontró la correspondencia con el folio proporcionado." });
            }

            return Ok(new { mensaje = "Correspondencia eliminada exitosamente." });
        }

        [HttpPut("editar/{folio}")]
        public async Task<IActionResult> EditarCorrespondencia(int folio, [FromBody] CorrespondenciaDto correspondenciaDto)
        {
            if (correspondenciaDto == null || folio != correspondenciaDto.Folio)
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



    }
}
