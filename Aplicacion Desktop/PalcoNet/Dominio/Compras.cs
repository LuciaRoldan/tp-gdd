using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalcoNet.Dominio
{
    public class Compra
    {
        Publicacion publicacion;
        MedioDePago medioDePago;
        float importe;
        List<Ubicacion> ubicaciones;

        internal List<Ubicacion> Ubicaciones
        {
            get { return ubicaciones; }
            set { ubicaciones = value; }
        }

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

        internal DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }

        internal float Importe
        {
            get { return importe; }
            set { importe = value; }
        }


    }
}
