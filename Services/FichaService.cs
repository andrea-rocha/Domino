using System.Collections.Generic;
using Domino.Models;
using Domino.Repositories;

namespace Domino.Services
{
    public class FichaService : IFichaService
    {
        public Ficha Create(Ficha ficha)
        {
            ficha.Nombre = ficha.Nombre.Trim();
            FichaRepositorio.Fichas.Add(ficha);
            return ficha;
        }

        public Ficha Get(string nombre)
        {
            var ficha = FichaRepositorio.Fichas.Find(f => f.Nombre == nombre);
            return ficha;
        }

        public List<Ficha> List()
        {
            return FichaRepositorio.Fichas;
        }

        public Ficha Update(Ficha fichaActualizada)
        {
            var fichaOld = FichaRepositorio.Fichas.FirstOrDefault(f => f.Nombre == fichaActualizada.Nombre);

            if (fichaOld == null)
            {
                return null;
            }
            fichaOld.Nombre = fichaActualizada.Nombre;
            fichaOld.Valores= fichaActualizada.Valores;
           // FichaRepositorio.Fichas[index] = fichaActualizada;
            return fichaActualizada;
        }

        public bool Delete(string nombre)
        {
            var ficha = FichaRepositorio.Fichas.Find(f => f.Nombre == nombre);

            if (ficha == null)
            {
                return false;
            }

            FichaRepositorio.Fichas.Remove(ficha);
            return true;
        }


        public List<Ficha> OrdenarFichas(List<Ficha> fichas)
        {
            List<Ficha> resultado = new List<Ficha>();

            Ficha fichaActual = fichas[0];
            fichas.RemoveAt(0);
            resultado.Add(fichaActual);

            while (fichas.Count > 0)
            {
                int valorBuscado = fichaActual.Valores[1];

                int i = 0;
                bool encontrado = false;

                while (i < fichas.Count && !encontrado)
                {
                    if (fichas[i].Valores[0] == valorBuscado)
                    {
                        encontrado = true;
                    }
                    else if (fichas[i].Valores[1] == valorBuscado)
                    {
                        int temp = fichas[i].Valores[0];
                        fichas[i].Valores[0] = fichas[i].Valores[1];
                        fichas[i].Valores[1] = temp;
                        encontrado = true;
                    }
                    else
                    {
                        i++;
                    }
                }

                if (!encontrado)
                {
                    throw new ArgumentException("No se puede construir una secuencia válida con las fichas dadas");
                }

                fichaActual = fichas[i];
                fichas.RemoveAt(i);
                resultado.Add(fichaActual);
            }

            return resultado;
        }




    }
}
