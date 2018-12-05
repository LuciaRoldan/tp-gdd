using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalcoNet.Dominio
{
    class Ubicacion
    {
        string tipoAsiento;

        public string TipoAsiento
        {
            get { return tipoAsiento; }
            set { tipoAsiento = value; }
        }
        bool numerada;

        public bool Numerada
        {
            get { return numerada; }
            set { numerada = value; }
        }
        decimal precio;

        public decimal Precio
        {
            get { return precio; }
            set { precio = value; }
        }

        List<Asiento> asientos = new List<Asiento>();

        internal List<Asiento> Asientos
        {
            get { return asientos; }
            set { asientos = value; }
        }
    }
}
