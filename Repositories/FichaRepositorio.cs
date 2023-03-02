using Domino.Models;

namespace Domino.Repositories
{
    public class FichaRepositorio
    {
        public static List<Ficha> Fichas = new()
        {
            new Ficha { Nombre = "ficha1", Valores = new int[] {2, 1} },
            new Ficha { Nombre = "ficha2", Valores = new int[] {2, 3} },
            new Ficha { Nombre = "ficha3", Valores = new int[] {1, 3} }
        };
    }
}
