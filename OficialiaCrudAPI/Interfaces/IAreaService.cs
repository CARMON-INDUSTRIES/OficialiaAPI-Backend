using OficialiaCrudAPI.DTO;

namespace OficialiaCrudAPI.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

    public interface IAreaService
    {
        Task<List<AreaDto>> ObtenerAreas();
    }

