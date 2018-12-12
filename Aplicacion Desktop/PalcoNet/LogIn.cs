using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using PalcoNet.Dominio;
using PalcoNet.Registro_de_Usuario;

namespace PalcoNet
{
    public partial class LogIn : MiForm
    {
        
        Sesion sesion = Sesion.getInstance();
        Usuario usuario = new Usuario();

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
            //encripta la contraseña que ingresa la persona para compararla con la encriptada que se encuentra en la base
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
                SqlDataReader r = servidor.query("EXEC verificarLogin_sp '" + textBox1.Text.Trim() + "', '" + Sb.ToString() + "'");
                usuario.NombreUsuario = textBox1.Text.ToString();
                while (r.Read())
                {
                    usuario.DebeCambiarContraseña = bool.Parse(r["debe_cambiar_pass"].ToString());
                }

                //en el caso de que el usurio haya sido registrado por un administrador o en el login en el primer
                //ingreso deberá cambiar su contraseña obligatoriamente

                if (usuario.DebeCambiarContraseña) {
                    CambioContrasenia c = new CambioContrasenia(usuario.NombreUsuario);
                    c.TopMost = true;
                    c.ShowDialog();
                }

                sesion.usuario = this.Usuario;
                List<Rol> roles = new List<Rol>();

               
                SqlDataReader reader = servidor.query("EXEC dbo.getRolesDeUsuario_sp '" + sesion.usuario.NombreUsuario + "'");


                while (reader.Read())
                {
                    Rol rol = new Rol();
                    string[] tokens = reader["nombre"].ToString().Split(' ');
                    rol.Nombre = tokens[0];
                    roles.Add(rol);
                }
                reader.Close();

                if (roles.Count() > 1)
                {
                    this.Hide();
                    new SeleccionarRol().ShowDialog();
                    this.Close();
                }
                else
                {
                    Console.Write("EL USUARIO ES: " + sesion.usuario.NombreUsuario);
                   // Console.Write("EL ROL ES: " + sesion.rol.Nombre);
                    sesion.rol = roles[0];
                    this.Hide();
                    new SeleccionarFuncionalidad().Show();
                    //this.Close();
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBoxUsuario.SelectedIndex > -1){
                switch (comboBoxUsuario.Text) { 
                    case "Cliente":
                        new RegistroDeCliente(new Cliente(), this).Show();
                        break;
                    case "Empresa":
                        new RegistroDeEmpresa(new Empresa(), this).Show();
                        break;
                }
                this.Hide();
            }else {
                MessageBox.Show("Se debe seleccionar el tipo de usuario que se quiere crear.", "Error", MessageBoxButtons.OK);
            }
        }
    }
}
