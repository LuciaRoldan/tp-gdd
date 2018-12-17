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
using PalcoNet.Abm_Rol;
using PalcoNet.Abm_Cliente;
using PalcoNet.Registro_de_Usuario;
using PalcoNet.Abm_Empresa_Espectaculo;
using PalcoNet.Abm_Grado;
using PalcoNet.Generar_Publicacion;
using PalcoNet.Editar_Publicacion;
using PalcoNet.Comprar;
using PalcoNet.Historial_Cliente;
using PalcoNet.Canje_Puntos;
using PalcoNet.Generar_Rendicion_Comisiones;
using PalcoNet.Listado_Estadistico;
using PalcoNet.Dominio;
using PalcoNet.Abm_Rubro;

namespace PalcoNet
{
    public partial class SeleccionarFuncionalidad : MiForm
    {
        Sesion sesion = Sesion.getInstance();
        Servidor servidor = Servidor.getInstance();
              
        public SeleccionarFuncionalidad()
        {
            //Trae las funcionalidades disponibles segun el rol con el que se loggeo y las carga para verlas en el combobox

            InitializeComponent();


            if (sesion.rol.Nombre == "Empresa" || sesion.rol.Nombre == "Cliente")
            {
                SqlDataReader reader = servidor.query("EXEC MATE_LAVADO.getFuncionalidadesDeRol_sp '" + sesion.rol.Nombre + "'");

                while (reader.Read())
                {
                    comboBox1.Items.Add(reader["nombre"].ToString());
                }
                reader.Close();
            }
            else
            {
                SqlDataReader reader2 = servidor.query("EXEC MATE_LAVADO.getFuncionalidadesDeUsuario_sp '" + sesion.usuario.NombreUsuario + "'");

                while (reader2.Read())
                {
                    comboBox1.Items.Add(reader2["nombre"].ToString());
                }
                reader2.Close();
            }
         }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Para cerrar la sesion se abre el login nuevamente
            new LogIn().Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {           
            //Segun la funcionalidad que se seleccione se elige que pantalla abrir
            try
            {
                switch (comboBox1.Text.ToString())
                {
                    case "ABM de rol":
                        new AbmRol(this).Show();
                        this.Hide();
                        break;
                    case "Registro de usuario":
                        new RegistroDeUsuario1(this).Show();
                        this.Hide();
                        break;
                    case "ABM de cliente":
                        new BusquedaCli(this).Show();
                        this.Hide();
                        break;
                    case "ABM de empresa de espectaculos":
                        new BusquedaEmp(this).Show();
                        this.Hide();
                        break;
                    case "ABM de rubro":
                        new AbmRubro(this).Show();
                        this.Hide();
                        break;
                    case "ABM grado de publicacion":
                        new GradoDeLaPublicacion(this).Show();
                        this.Hide();
                        break;
                    case "Generar publicacion":
                        new CrearPublicacion(this).Show();
                        this.Hide();
                        break;
                    case "Editar publicacion":
                        new Editar_publicacion(this).Show();
                        this.Hide();
                        break;
                    case "Comprar":
                        new BuscarP(this).Show();
                        this.Hide();
                        break;
                    case "Historial del cliente":
                        new Historial(this).Show();
                        this.Hide();
                        break;
                    case "Canje y administracion de puntos":
                        new CanjePuntos(this).Show(); 
                        this.Hide();
                        break;
                    case "Generar pago de comisiones":
                        new Rendicion(this).Show();
                        this.Hide();
                        break;
                    case "Listado estadistico":
                        this.Hide();
                        new Listados(this).Show();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
