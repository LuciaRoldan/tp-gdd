using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PalcoNet.Listado_Estadistico
{
    public partial class Listados : Form
    {
        public Listados()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new SeleccionarFuncionalidad().Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new ClientesMuchasCompras().Show();
            this.Hide(); 
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new EmpresaLocalidades().Show();
            this.Hide(); 
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new ClientesPuntos().Show();
            this.Hide(); 
        }
    }
}
