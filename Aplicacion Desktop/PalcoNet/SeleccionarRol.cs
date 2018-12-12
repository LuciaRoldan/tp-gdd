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
            //Se traen los roles que tiene el usuario y se le da a elegir cual quiere utilizar
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
            //Cuando se selecciona un rol se lo guarda en la sesion y se abre la siguiente pantalla
            sesion.rol.Nombre = comboBox1.Text.ToString();
            new SeleccionarFuncionalidad().Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
