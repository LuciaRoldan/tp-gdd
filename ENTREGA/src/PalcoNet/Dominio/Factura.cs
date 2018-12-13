using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalcoNet.Dominio
{
    class Factura
    {
        Empresa empresa;
        DateTime fechaDeFacturacion;
        decimal importeTotal;

        internal Empresa Empresa
        {
            get { return empresa; }
            set { empresa = value; }
        }
        
        public DateTime FechaDeFacturacion
        {
            get { return fechaDeFacturacion; }
            set { fechaDeFacturacion = value; }
        }
        
        public decimal ImporteTotal
        {
            get { return importeTotal; }
            set { importeTotal = value; }
        }
    }
}
