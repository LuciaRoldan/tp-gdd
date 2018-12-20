using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using PalcoNet.Dominio;
using System.Security.Cryptography;

namespace PalcoNet.Registro_de_Usuario
{
    public partial class RegistroDeUsuario1 : MiForm
    {
        public RegistroDeUsuario1(MiForm formAnterior): base(formAnterior)
        {
            InitializeComponent();
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        //Se verifica que esten todos los campos completos
        private bool camposCompletos() {
            string errores = "";
            
            if (string.IsNullOrWhiteSpace(textBox1.Text)) { errores += "Se debe completar el nombre de usuario. \n"; }
            if (string.IsNullOrWhiteSpace(textBox2.Text)) { errores += "Se debe completar la contraseña. \n"; }
            if (comboBox1.SelectedItem == null) { errores += "Se debe seleccionar un tipo de usuario. \n"; }

            if (errores != ""){
                MessageBox.Show(errores, "Error", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
//completa los campos comunes a empresa y cliente y los guarda en usuario y luego se divide dependiendo el tipo 
//para los datos particulares de cada uno
        private void button2_Click(object sender, EventArgs e)
        {
            if (this.camposCompletos())
            {
                if (comboBox1.SelectedItem.Equals("Cliente"))
                {
                    Cliente cliente = new Cliente();
                    cliente.NombreUsuario = textBox1.Text;

                    StringBuilder Sb = new StringBuilder();
                    using (SHA256 hash = SHA256Managed.Create())
                    {
                        Encoding enc = Encoding.UTF8;
                        Byte[] result = hash.ComputeHash(enc.GetBytes(textBox2.Text.ToString()));

                        foreach (Byte b in result)
                            Sb.Append(b.ToString("x2"));

                        Console.WriteLine("EL HASH ES:" + Sb.ToString());
                    }
                    cliente.Contrasenia = Sb.ToString();

                    new RegistroDeCliente(cliente,  this).Show();
                    this.Hide();
                }
                else
                {
                    if (comboBox1.SelectedItem.Equals("Empresa"))
                    {
                        Empresa empresa = new Empresa();
                        empresa.NombreUsuario = textBox1.Text;

                        StringBuilder Sb = new StringBuilder();
                        using (SHA256 hash = SHA256Managed.Create())
                        {
                            Encoding enc = Encoding.UTF8;
                            Byte[] result = hash.ComputeHash(enc.GetBytes(textBox2.Text.ToString()));

                            foreach (Byte b in result)
                                Sb.Append(b.ToString("x2"));

                            Console.WriteLine("EL HASH ES:" + Sb.ToString());
                        }
                        empresa.Contrasenia = Sb.ToString();

                        new RegistroDeEmpresa(empresa, this).Show();
                        this.Hide();
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.cerrarAnteriores();
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
