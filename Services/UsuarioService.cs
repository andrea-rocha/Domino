using Domino.Models;
using Domino.Repositories;

namespace Domino.Services
{
    public class UsuarioService : IUsuarioService
    {
        public usuario Get(login userLogin)
        {
           usuario user = usuarioRepositorio.Usuarios.FirstOrDefault(o => o.NombreUsuario.Equals(userLogin.NombreUsuario, StringComparison.OrdinalIgnoreCase) && o.Contraseña.Equals(userLogin.Contraseña));

            return user;
        }
    }
}
