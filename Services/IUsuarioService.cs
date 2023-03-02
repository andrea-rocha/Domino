using Domino.Models;

namespace Domino.Services
{
    public interface IUsuarioService
    {
        public usuario Get(login userLogin);
    }
}
