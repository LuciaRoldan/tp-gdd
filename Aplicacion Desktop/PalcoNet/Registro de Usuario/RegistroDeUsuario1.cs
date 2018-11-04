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
    public partial class RegistroDeUsuario1 : Form
    {
        public RegistroDeUsuario1()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.Equals("Cliente")){
                //Hay que guardar los datos del usuario en la base

                new RegistroDeCliente().Show();
                this.Hide();
            }
            else {
                if (comboBox1.SelectedItem.Equals("Empresa")){
                    //Aca hay que guardar los datos del usuario

                    new RegistroDeEmpresa().Show();
                    this.Hide();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new SeleccionarFuncionalidad().Show();
            this.Hide();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.Write(comboBox1.SelectedItem);
        }
    }
}
