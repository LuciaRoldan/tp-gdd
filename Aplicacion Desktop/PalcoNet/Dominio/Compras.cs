using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalcoNet.Dominio
{
    class Compras
    {
        Publicacion publicacion;
        MedioDePago medioDePago;
        float importe;

        internal Publicacion Publicacion
        {
            get { return publicacion; }
            set { publicacion = value; }
        }

        internal MedioDePago MedioDePago
        {
            get { return medioDePago; }
            set { medioDePago = value; }
        }
        DateTime fecha;

        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }

        public float Importe
        {
            get { return importe; }
            set { importe = value; }
        }


    }
}
