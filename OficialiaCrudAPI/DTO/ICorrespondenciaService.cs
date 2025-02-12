namespace OficialiaCrudAPI.DTO
{
    public interface ICorrespondenciaService
    {
        Task<List<CorrespondenciaDto>> ObtenerCorrespondencias();
        Task<bool> RegistrarCorrespondencia(CorrespondenciaDto correspondenciaDto);
        Task<bool> EliminarCorrespondencia(int folio);
        Task<bool> EditarCorrespondencia(CorrespondenciaDto correspondenciaDto);


    }
}
