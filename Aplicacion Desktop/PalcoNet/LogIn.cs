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
using System.Security.Cryptography;
using PalcoNet.Dominio;

namespace PalcoNet
{
    public partial class LogIn : Form
    {
        
        Sesion sesion;
        Usuario usuario;

        public Usuario Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }
        public LogIn()
        {
            this.Usuario = usuario;
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonAceptar_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Servidor servidor = Servidor.getInstance();
            StringBuilder Sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(textBox2.Text.ToString()));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));

                Console.WriteLine("EL HASH ES:" + Sb);
            }
            try
            {

                servidor.realizarQuery("EXEC verificarLogin_sp '" + textBox1.Text.Trim() + "', '" + Sb.ToString() + "'");
                Sesion s = Sesion.getInstance();
                s.usuario = textBox1.Text.Trim();
                List<String> roles = new List<String>();

                String query = "SELECT r.nombre FROM UsuarioXRol ur JOIN Usuarios u ON (u.id_usuario = ur.id_usuario) JOIN Roles r ON (r.id_rol = ur.id_rol) WHERE username like ' " + s.usuario + "'";

                SqlDataReader reader = servidor.query(query);

                while (reader.Read())
                {
                    String rol;
                    rol = reader["nombre"].ToString();
                    roles.Add(rol);
                }
                reader.Close();

                if (roles.Count() > 1)
                {
                      new SeleccionarRol().Show();
                  }
                  else
                {
                      new SeleccionarFuncionalidad().Show();
                }
            }
            catch (SqlException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
