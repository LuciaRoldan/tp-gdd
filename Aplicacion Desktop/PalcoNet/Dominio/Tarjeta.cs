using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalcoNet.Dominio
{
    class Tarjeta
    {
        int numeroDeTarjeta;
        string titular;

        public int NumeroDeTarjeta
        {
            get { return numeroDeTarjeta; }
            set { numeroDeTarjeta = value; }
        }
        
        public string Titular
        {
            get { return titular; }
            set { titular = value; }
        } 
    }
}
