using OficialiaCrudAPI.DTO;

namespace OficialiaCrudAPI.Interfaces
{
    public interface IImportanciaService
    {
        Task<List<ImportanciaDto>> ObtenerImportancias();
    }
}
