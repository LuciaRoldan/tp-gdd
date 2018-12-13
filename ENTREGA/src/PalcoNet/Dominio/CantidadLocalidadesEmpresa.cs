using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalcoNet.Dominio
{
    class CantidadLocalidadesEmpresa
    {
        string empresa;

        public string Empresa
        {
            get { return empresa; }
            set { empresa = value; }
        }
        long cuit;

        public long Cuit
        {
            get { return cuit; }
            set { cuit = value; }
        }
        int localidadesNoVendidas;

        public int LocalidadesNoVendidas
        {
            get { return localidadesNoVendidas; }
            set { localidadesNoVendidas = value; }
        }
    }
}
