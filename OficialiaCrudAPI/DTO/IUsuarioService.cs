namespace OficialiaCrudAPI.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IUsuarioService
{
    Task<List<UsuarioDto>> ObtenerUsuarios();
}
