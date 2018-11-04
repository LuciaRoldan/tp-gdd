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
    public partial class FinalizarCompra : Form
    {
        public FinalizarCompra()
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
            //hay que guardar las cosas en la base

            new Ubicaciones().Show();
            this.Hide();
        }
    }
}
