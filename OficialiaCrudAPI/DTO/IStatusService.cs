namespace OficialiaCrudAPI.DTO
{
    public interface IStatusService
    {
        Task<List<StatusDto>> ObtenerStatus();
    }
}
