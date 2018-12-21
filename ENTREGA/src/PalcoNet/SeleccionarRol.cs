using PalcoNet.Dominio;
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
using PalcoNet.Registro_de_Usuario;
using PalcoNet.Abm_Cliente;
using PalcoNet.Abm_Empresa_Espectaculo;

namespace PalcoNet
{
    public partial class SeleccionarRol : MiForm
    {
        Sesion sesion = Sesion.getInstance();
        Servidor servidor = Servidor.getInstance();
        public SeleccionarRol()
        {
            //Se traen los roles que tiene el usuario y se le da a elegir cual quiere utilizar
            InitializeComponent();
            
            SqlDataReader reader = servidor.query("EXEC MATE_LAVADO.getRolesDeUsuario_sp '" + sesion.usuario.NombreUsuario + "'");

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
            String rol = comboBox1.Text.Substring(0, 3);

            switch (rol)
            {
                case "Cli":

                    sesion.rol.Nombre = "Cliente";
                    new SeleccionarFuncionalidad().Show();
                    this.Hide();
                    break;

                case "Emp":
                    
                    sesion.rol.Nombre = "Empresa";
                    new SeleccionarFuncionalidad().Show();
                    this.Hide();
                    break;
                
                default:
                    sesion.rol.Nombre = this.comboBox1.Text;
                    new SeleccionarFuncionalidad().Show();
                    this.Hide();
                    break;
            }
        }

        //Se guarda el rol en la sesion
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            sesion.rol.Nombre = comboBox1.Text;
        }
    }
}
