using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PalcoNet.Dominio
{
    public class Tarjeta
    {
        long numeroDeTarjeta;
        string titular;
        int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public long NumeroDeTarjeta
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
