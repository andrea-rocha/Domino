using Domino.Models;

namespace Domino.Repositories
{
    public class usuarioRepositorio
    {
        public static List<usuario> Usuarios = new()
        {
            new() { NombreUsuario = "admin", Correo = "admin@email.com", Contraseña = "123456", Nombre = "Andrea", Apellido = "Rocha", Rol = "Administrator" },
            new() { NombreUsuario = "pedro.standard", Correo = "pedro.standard@email.com", Contraseña = "P_12345", Nombre = "Pedro", Apellido = "Perez", Rol = "Standard" },
        };
    }
}
