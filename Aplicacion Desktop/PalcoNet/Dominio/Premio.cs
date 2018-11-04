using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalcoNet.Dominio
{
    class Premio
    {
        int cantidadDePuntos;
        DateTime fechaDeVencimiento;

        public int CantidadDePuntos
        {
            get { return cantidadDePuntos; }
            set { cantidadDePuntos = value; }
        }
      
        public DateTime FechaDeVencimiento
        {
            get { return fechaDeVencimiento; }
            set { fechaDeVencimiento = value; }
        }


    }
}
