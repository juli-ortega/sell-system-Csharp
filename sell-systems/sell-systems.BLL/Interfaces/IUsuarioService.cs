    using sell_systems.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sell_systems.BLL.Interfaces
{
    public interface IUsuarioService
    {
        Task<List<Usuario>> lista();
        Task<Usuario> crear(Usuario entidad, Stream foto = null, string nombreFoto = "", string urlPlantillaCorreo= "");
        Task<Usuario> editar(Usuario entidad, Stream foto = null, string nombreFoto = "");
        Task<bool> eliminar(int idUsuario);
        Task<Usuario> obtenerPorCredenciales(string correo, string clave);
        Task<Usuario> obtenerPorId(int idUsuario);
        Task<bool> guardarPerfil(Usuario entidad);
        Task<bool> cambiarClave(int idUsuario, string claveActual, string claveNueva);
        Task<bool> restablecerClave(string correo, string UrlPlantillaCorreo);

    }
}
