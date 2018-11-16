using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using PalcoNet.Registro_de_Usuario;
using PalcoNet.Abm_Cliente;
using PalcoNet.Dominio;
using PalcoNet.Abm_Empresa_Espectaculo;
using PalcoNet.Abm_Grado;
using PalcoNet.Abm_Rol;

namespace PalcoNet
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new RegistroDeUsuario1(null));
        }
    }
}
