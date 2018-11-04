using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PalcoNet.Registro_de_Usuario
{
    public partial class RegistroDeCliente : Form
    {
        public RegistroDeCliente()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new SeleccionarFuncionalidad().Show();
            this.Hide();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            new RegistroDeUsuario1().Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Hay que seguir guardando los datos del usuario
            new RegistroDomicilio().Show();
            this.Hide();
        }
    }
}
