using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalcoNet.Dominio
{
    class Puntos
    {
        string descripcion;
        int puntos;

        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        public int CantidadDePuntos
        {
            get { return puntos; }
            set { puntos = value; }
        }
    }
}
