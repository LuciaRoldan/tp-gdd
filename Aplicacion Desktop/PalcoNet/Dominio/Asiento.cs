using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalcoNet.Dominio
{
    class Asiento
    {
        char fila;

        public char Fila
        {
            get { return fila; }
            set { fila = value; }
        }
        int asiento;

        public int Asiento1
        {
            get { return asiento; }
            set { asiento = value; }
        }

        int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}
