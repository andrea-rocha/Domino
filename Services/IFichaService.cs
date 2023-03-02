using Domino.Models;
using Domino.Services;

namespace Domino.Services
{
    public interface IFichaService
    {
        Ficha Create(Ficha ficha);
        Ficha Get(string nombre);
        List<Ficha> List();
        Ficha Update(Ficha fichaActualizada);
        bool Delete(string nombre);
        List<Ficha> OrdenarFichas(List<Ficha> fichas);
        //List<Ficha> OrdenarFichas(List<Ficha> cadena, List<Ficha> fichasRestantes);

    }
}
