using OficialiaCrudAPI.Models;

namespace OficialiaCrudAPI.Interfaces
{
    public interface ICorrespondenciaService
    {
        Task<List<CorrespondenciaDto>> ObtenerCorrespondencias(string userId);
        Task<bool> RegistrarCorrespondencia(CorrespondenciaDto correspondenciaDto);
        Task<bool> EliminarCorrespondencia(int id);
        Task<bool> EditarCorrespondencia(CorrespondenciaDto correspondenciaDto);
        Task<int> ObtenerNuevasCorrespondencias(DateTime ultimaFecha);
        Task<List<CorrespondenciaDto>> ObtenerTodasLasCorrespondencias();

    }
}
