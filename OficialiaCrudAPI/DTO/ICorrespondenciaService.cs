namespace OficialiaCrudAPI.DTO
{
    public interface ICorrespondenciaService
    {
        Task<List<CorrespondenciaDto>> ObtenerCorrespondencias();
        Task<bool> RegistrarCorrespondencia(CorrespondenciaDto correspondenciaDto);

    }
}
