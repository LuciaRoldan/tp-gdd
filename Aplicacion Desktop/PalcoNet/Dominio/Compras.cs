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
        Tarjeta medioDePago;
        decimal importe;
        List<Ubicacion> ubicaciones;
        int cantidadEntradas;
        int id;
        decimal comision;
        Espectaculo espectaculo;

        internal Espectaculo Espectaculo
        {
            get { return espectaculo; }
            set { espectaculo = value; }
        }

        public decimal Comision
        {
            get { return comision; }
            set { comision = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int CantidadEntradas
        {
            get { return cantidadEntradas; }
            set { cantidadEntradas = value; }
        }

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

        internal Tarjeta MedioDePago
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

        internal decimal Importe
        {
            get { return importe; }
            set { importe = value; }
        }

        public decimal calcularImporte()
        {
            decimal total = 0;
            foreach (Ubicacion u in this.Ubicaciones) { total += u.Precio * u.CantidadAsientos; }
            return total;
        }

        public int calcularCantidadAsientos()
        {
            int cantidad = 0;
            foreach (Ubicacion u in this.Ubicaciones) { cantidad += u.CantidadAsientos; }
            return cantidad;
        }


    }
}
