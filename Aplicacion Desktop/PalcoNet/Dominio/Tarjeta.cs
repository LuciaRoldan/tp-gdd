using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalcoNet.Dominio
{
    public class Tarjeta
    {
        int numeroDeTarjeta;
        string titular;
        int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

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
