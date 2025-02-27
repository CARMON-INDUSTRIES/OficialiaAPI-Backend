namespace OficialiaCrudAPI.Interfaces
{
    public interface ICorrespondenciaService
    {
        Task<List<CorrespondenciaDto>> ObtenerCorrespondencias();
        Task<bool> RegistrarCorrespondencia(CorrespondenciaDto correspondenciaDto);
        Task<bool> EliminarCorrespondencia(int id);
        Task<bool> EditarCorrespondencia(CorrespondenciaDto correspondenciaDto);

    }
}
