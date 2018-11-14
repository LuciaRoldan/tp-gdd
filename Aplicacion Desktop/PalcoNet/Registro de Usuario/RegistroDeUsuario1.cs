using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PalcoNet.Dominio;

namespace PalcoNet.Registro_de_Usuario
{
    public partial class RegistroDeUsuario1 : MiForm
    {
        public RegistroDeUsuario1(MiForm formAnterior): base(formAnterior)
        {
            InitializeComponent();
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private bool camposCompletos() {
            return !string.IsNullOrWhiteSpace(textBox1.Text)
                && !string.IsNullOrWhiteSpace(textBox2.Text)
                && comboBox1.SelectedItem != null;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.camposCompletos())
            {
                if (comboBox1.SelectedItem.Equals("Cliente"))
                {
                    Cliente cliente = new Cliente();
                    cliente.NombreUsuario = textBox1.Text;
                    cliente.Contrasenia = textBox2.Text;
                    new RegistroDeCliente(cliente,  this).Show();
                    this.Hide();
                }
                else
                {
                    if (comboBox1.SelectedItem.Equals("Empresa"))
                    {
                        Empresa empresa = new Empresa();
                        empresa.NombreUsuario = textBox1.Text;
                        empresa.Contrasenia = textBox2.Text;
                        new RegistroDeEmpresa(empresa, this).Show();
                        this.Hide();
                    }
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
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
