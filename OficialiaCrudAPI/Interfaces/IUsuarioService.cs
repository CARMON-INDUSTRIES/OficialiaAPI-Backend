using OficialiaCrudAPI.DTO;

namespace OficialiaCrudAPI.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IUsuarioService
{
    Task<List<UsuarioDto>> ObtenerUsuarios();
    Task EliminarUsuarioAreaPorUsuarioId(string userId);
    Task<UsuarioAreaDto> ObtenerUsuarioArea(string userId);
}
