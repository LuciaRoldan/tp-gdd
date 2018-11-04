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
    public partial class Seleccion : Form
    {
        public Seleccion()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new BuscarP().Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Ubicacioines().Show();
            this.Hide();
        }
    }
}
