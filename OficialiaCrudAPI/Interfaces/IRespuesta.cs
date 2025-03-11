using OficialiaCrudAPI.DTO;

namespace OficialiaCrudAPI.Interfaces
{
    public interface IRespuesta
    {
        Task<List<RespuestaDto>> ObtenerRespuesta();
        Task<RespuestaDto?> ObtenerRespuestaPorId(int id);
        Task<bool> CrearRespuesta(RespuestaDto respuestaDto);

    }
}
