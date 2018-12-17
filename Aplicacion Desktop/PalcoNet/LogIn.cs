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
using PalcoNet.Abm_Cliente;
using PalcoNet.Abm_Empresa_Espectaculo;

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

            }
            try
            {
                SqlDataReader r = servidor.query("EXEC MATE_LAVADO.verificarLogin_sp '" + textBox1.Text.Trim() + "', '" + Sb.ToString() + "'");
                usuario.NombreUsuario = textBox1.Text.ToString();
                while (r.Read())
                {
                    usuario.DebeCambiarContraseña = bool.Parse(r["debe_cambiar_pass"].ToString());
                    usuario.IdUsuario = int.Parse(r["id_usuario"].ToString());
                }

             

                //en el caso de que el usurio haya sido registrado por un administrador o en el login en el primer
                //ingreso deberá cambiar su contraseña obligatoriamente

                if (usuario.DebeCambiarContraseña)
                {
                    CambioContrasenia c = new CambioContrasenia(usuario.NombreUsuario);
                    c.TopMost = true;
                    c.ShowDialog();
                }

                sesion.usuario = this.Usuario;
                Console.Write(sesion.usuario.IdUsuario);
                List<Rol> roles = new List<Rol>();


                SqlDataReader reader = servidor.query("EXEC MATE_LAVADO.getRolesDeUsuario_sp '" + sesion.usuario.NombreUsuario + "'");


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
                    Console.Write("hay mas d un rol");
                    this.Hide();
                    new SeleccionarRol().ShowDialog();
                    this.Close();
                }
                else
                {
                    Console.Write("EL USUARIO ES: " + sesion.usuario.NombreUsuario);
                    // Console.Write("EL ROL ES: " + sesion.rol.Nombre);
                    sesion.rol = roles[0];


                    //Verificamos que el usuario, si es Cliente o Empresa, tenga toda la información correspondiente completa
                    switch (sesion.rol.Nombre)
                    {
                        case "Cliente":
                            Cliente cli = new Cliente();
                            SqlDataReader reader2 = servidor.query("EXEC MATE_LAVADO.elClienteTieneInfoCompleta_sp '" + sesion.usuario.IdUsuario + "'");
                            reader2.Read();
                            if (bool.Parse(reader2["esta_completa"].ToString()))
                            {
                                new SeleccionarFuncionalidad().Show();
                                this.Hide();
                            }
                            else
                            {
                                cli = Sesion.getInstance().traerCliente();
                                new ModificarCli(cli, this).Show();
                                this.Hide();
                            }
                            break;

                        case "Empresa":
                            Empresa emp = new Empresa();
                            SqlDataReader reader4 = servidor.query("EXEC MATE_LAVADO.laEmpresaTieneInfoCompleta_sp '" + sesion.usuario.NombreUsuario + "'");
                            reader4.Read();
                            if (bool.Parse(reader4["esta_completa"].ToString()))
                            {
                                new SeleccionarFuncionalidad().Show();
                                this.Hide();
                            }
                            else
                            {
                                emp = Sesion.getInstance().traerEmpresa();
                                new ModificarEmp(emp, this).Show();
                                this.Hide();
                            }
                            break;
                        default:
                            this.Hide();
                            new SeleccionarFuncionalidad().Show();
                            break;
                    }
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
