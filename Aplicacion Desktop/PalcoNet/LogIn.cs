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

namespace PalcoNet
{
    public partial class LogIn : Form
    {
        public static String usuario { get; set; }
        Sesion sesion;
        public LogIn()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonAceptar_Click(object sender, EventArgs e)
        {
            usuario = txtUsuario.Text.Trim();
            if (validarDatos())
            {
                {
                    Servidor servidor = Servidor.getInstance();
                    StringBuilder Sb = new StringBuilder();
                    using (SHA256 hash = SHA256Managed.Create())
                    {
                        Encoding enc = Encoding.UTF8;
                        Byte[] result = hash.ComputeHash(enc.GetBytes(txtPassword.Text.ToString()));

                        foreach (Byte b in result)
                            Sb.Append(b.ToString("x2"));
                    }
                    try
                    {

                        servidor.realizarQuery("EXEC GESTIONAME_LAS_VACACIONES.LoguearUsuario '" + txtUsuario.Text.Trim() + "', '" + Sb.ToString() + "'");
                        Sesion s = Sesion.getInstance();
                        s.usuario = txtUsuario.Text.Trim();
                        SqlDataReader reader = servidor.query("select id from GESTIONAME_LAS_VACACIONES.Pacientes where usuario like '" + txtUsuario.Text.Trim() + "'");
                        if (reader.Read())
                        {
                            s.afiliado.id = Convert.ToInt32(reader[0]);
                            reader.Close();
                            s.afiliado = Abm_Afiliado.AfiliadoManager.BuscarUnAfiliado(s.afiliado.id);
                        }
                        else
                            reader.Close();
                        reader.Close();
                        reader = servidor.query("select id from GESTIONAME_LAS_VACACIONES.Profesionales where usuario like '" + txtUsuario.Text.Trim() + "'");
                        if (reader.Read())
                        {

                            s.profesional.matricula = Convert.ToInt32(reader[0]);
                            reader.Close();
                            s.profesional = Pedir_Turno.ProfesionalManager.BuscarUnProfesional(s.profesional.matricula);
                        }
                        else
                            reader.Close();

                        new ValidacionDeRol(txtUsuario.Text.Trim()).ShowDialog();

                    }
                    catch (SqlException ex)
                    {
                        System.Windows.Forms.MessageBox.Show(ex.Message);
                    }

                }
            }
            else
            {
                MessageBox.Show("Faltan algun dato");
            }
        }
    }
}
