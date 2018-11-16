using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalcoNet.Dominio
{
    class CompraCliente
    {

        string nombre;

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }
        string apellido;

        public string Apellido
        {
            get { return apellido; }
            set { apellido = value; }
        }
        string usuario;

        public string Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }
        string empresa;

        public string Empresa
        {
            get { return empresa; }
            set { empresa = value; }
        }
        int cantidadCompras;

        public int CantidadCompras
        {
            get { return cantidadCompras; }
            set { cantidadCompras = value; }
        }


    }
}
