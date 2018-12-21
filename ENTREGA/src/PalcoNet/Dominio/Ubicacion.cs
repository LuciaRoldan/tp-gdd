using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalcoNet.Dominio
{
    public class Ubicacion
    {
        string tipoAsiento;
        bool numerada;
        decimal precio;
        List<Asiento> asientos = new List<Asiento>();
        int cantidadAsientos;
        int cantidadFilas;
        int id;

        public Ubicacion(Ubicacion ubicacion, decimal asientosSolicitados)
        {
            this.tipoAsiento = ubicacion.tipoAsiento;
            this.numerada = ubicacion.numerada;
            this.precio = ubicacion.precio;
            this.asientos = ubicacion.asientos;
            this.cantidadFilas = ubicacion.cantidadFilas;
            this.id = ubicacion.id;
            this.cantidadAsientos = (int) asientosSolicitados;
        }

        public Ubicacion()
        {
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int CantidadFilas
        {
            get { return cantidadFilas; }
            set { cantidadFilas = value; }
        }

        public int CantidadAsientos
        {
            get { return cantidadAsientos; }
            set { cantidadAsientos = value; }
        }

        public string TipoAsiento
        {
            get { return tipoAsiento; }
            set { tipoAsiento = value; }
        }

        public bool Numerada
        {
            get { return numerada; }
            set { numerada = value; }
        }

        public decimal Precio
        {
            get { return precio; }
            set { precio = value; }
        }

        internal List<Asiento> Asientos
        {
            get { return asientos; }
            set { asientos = value; }
        }
    }
}
