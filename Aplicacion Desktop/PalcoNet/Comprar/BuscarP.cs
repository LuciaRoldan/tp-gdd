using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PalcoNet.Comprar
{
    public partial class BuscarP : Form
    {
        public BuscarP()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new SeleccionarFuncionalidad().Show();
            this.Hide(); 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Hay que hacer la query aca y obtener la lista para pasarla abajo

            new Seleccion().Show();
            this.Hide();
        }
    }
}
