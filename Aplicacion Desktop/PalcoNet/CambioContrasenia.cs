using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PalcoNet
{
    public partial class CambioContrasenia : Form
    {
        string username;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public CambioContrasenia(string username)
        {
            InitializeComponent();
            this.Username = username;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Aceptar_Click(object sender, EventArgs e)
        {
            string errores = "";
            if (string.IsNullOrWhiteSpace(textBoxUser.Text)) { errores += "Se debe completar el usuario. \n"; }
            if (string.IsNullOrWhiteSpace(textBoxPass.Text)) { errores += "Se debe completar la contraseña.\n"; }
            //Se encripta la nueva contraseña y se actualiza el nombre y contraseña del usuario en la base, 
            //indicando también que ya no será necesario modificarla.
            if (errores == "")
            {
                StringBuilder Sb = new StringBuilder();
                using (SHA256 hash = SHA256Managed.Create())
                {
                    Encoding enc = Encoding.UTF8;
                    Byte[] result = hash.ComputeHash(enc.GetBytes(textBoxPass.Text.ToString()));

                    foreach (Byte b in result)
                        Sb.Append(b.ToString("x2"));

                    Console.WriteLine("EL HASH ES:" + Sb);
                }
                string query = this.Username + "', '" + textBoxUser.Text + "', '" + Sb + "'";
                Servidor servidor = Servidor.getInstance();
                SqlDataReader reader = servidor.query("exec actualizarUsuarioYContrasenia_sp '" + query);

                Sesion.getInstance().usuario.NombreUsuario = textBoxUser.Text;
                MessageBox.Show("El nombre de usuario y constraseña se acualizaron exitosamente!", "Cambiar Contraseña", MessageBoxButtons.OK);
                this.Close();
            }
            else 
            {
                MessageBox.Show(errores, "Error", MessageBoxButtons.OK);
            }
        }
    }
}
