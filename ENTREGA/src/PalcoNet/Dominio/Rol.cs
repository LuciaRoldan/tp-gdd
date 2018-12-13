using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalcoNet.Dominio
{
    class Rol
    {
        List<Funcionalidad> funcionalidades;

        internal List<Funcionalidad> Funcionalidades
        {
            get { return funcionalidades; }
            set { funcionalidades = value; }
        }
        string nombre;

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }
    }
}
