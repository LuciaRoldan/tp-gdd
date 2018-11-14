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

namespace PalcoNet.Abm_Cliente
{
    public partial class BusquedaCli : Form
    {
        public BusquedaCli()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new SeleccionarFuncionalidad().Show();
            this.Hide(); 
        }

        private void button1_Click(object sender, EventArgs e)
        {   //SACARA ESE NULL
            new RegistroDeCliente(new Cliente(),null).Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Aca hay que agarrar los valores, buscar y pasarle los resultados a la ventana

            new ResultadoBusquedaCli().Show();
            this.Hide();
        }
    }
}
