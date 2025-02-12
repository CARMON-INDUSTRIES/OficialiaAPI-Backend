namespace OficialiaCrudAPI.DTO
{
    public interface IImportanciaService
    {
        Task<List<ImportanciaDto>> ObtenerImportancias();
    }
}
