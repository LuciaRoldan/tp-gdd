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
    public partial class MedioPago : Form
    {
        public MedioPago()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //new Ubicaciones().Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new FinalizarCompra().Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new NuevoMP().Show();
        }
    }
}
