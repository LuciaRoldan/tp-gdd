using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PalcoNet.Registro_de_Usuario;
using PalcoNet.Dominio;

namespace PalcoNet.Abm_Empresa_Espectaculo
{
    public partial class BusquedaEmp : Form
    {
        public BusquedaEmp()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new SeleccionarFuncionalidad().Show();
            this.Hide(); 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Aca hay que hacer que con los datos se busquen las empresas y se pase una lista abajo

            new ResultadosBusquedaEmp().Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {   //SACAR ESE NULL
            new RegistroDeEmpresa(new Empresa(), null).Show();
            this.Hide(); 
        }
    }
}
