using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PalcoNet
{
    public partial class SeleccionarRol : Form
    {
        Sesion sesion = Sesion.getInstance();
        Servidor servidor = Servidor.getInstance();
        public SeleccionarRol()
        {
            InitializeComponent();
            
            SqlDataReader reader = servidor.query("EXEC dbo.getRolesDeUsuario_sp '" + sesion.usuario.NombreUsuario + "'");

            while (reader.Read())
            {
                comboBox1.Items.Add(reader["nombre"].ToString());
            }
            reader.Close();
        
        }

        private void SeleccionarRol_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sesion.rol.Nombre = comboBox1.Text.ToString();
            new SeleccionarFuncionalidad().Show();
            //Falta asignar el rol a la sesion y pasar a la siguiente pantalla
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
