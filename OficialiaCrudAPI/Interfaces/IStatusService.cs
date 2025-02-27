using OficialiaCrudAPI.DTO;

namespace OficialiaCrudAPI.Interfaces
{
    public interface IStatusService
    {
        Task<List<StatusDto>> ObtenerStatus();
    }
}
