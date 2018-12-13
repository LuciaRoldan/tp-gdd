using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalcoNet.Dominio
{
    class ElementoHistorialCliente
    {
        string descripcion;

        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }
        DateTime fecha;

        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }
        decimal importe;

        public decimal Importe
        {
            get { return importe; }
            set { importe = value; }
        }
        int cantidadAsientos;

        public int CantidadAsientos
        {
            get { return cantidadAsientos; }
            set { cantidadAsientos = value; }
        }
        int numeroTarjeta;

        public int NumeroTarjeta
        {
            get { return numeroTarjeta; }
            set { numeroTarjeta = value; }
        }

    }
}
