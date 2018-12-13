using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalcoNet.Dominio
{
    public class ClientePuntosListado
    {
        string nombre;
        string apellido;
        int puntos;

        public int Puntos
        {
            get { return puntos; }
            set { puntos = value; }
        }

        public string Apellido
        {
            get { return apellido; }
            set { apellido = value; }
        }

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }
    }
}
