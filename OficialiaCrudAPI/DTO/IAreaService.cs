namespace OficialiaCrudAPI.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

    public interface IAreaService
    {
        Task<List<AreaDto>> ObtenerAreas();
    }

