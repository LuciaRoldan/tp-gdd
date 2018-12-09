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

namespace PalcoNet
{
    public partial class SeleccionarFuncionalidad : MiForm
    {
        Sesion sesion = Sesion.getInstance();
        Servidor servidor = Servidor.getInstance();
              
        public SeleccionarFuncionalidad()
        {
            InitializeComponent();
            
            SqlDataReader reader = servidor.query("EXEC dbo.getFuncionalidadesDeUsuario_sp '" + sesion.usuario.NombreUsuario + "'");

            while (reader.Read())
            {
              comboBox1.Items.Add(reader["nombre"].ToString());
            }
            reader.Close();
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
            new LogIn().Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {           

            try
            {
                switch (comboBox1.Text.ToString())
                {
                    case "ABM de rol":
                        new AbmRol(this).Show();
                        break;
                    case "Registro de usuario":
                        new RegistroDeUsuario1(this).Show();
                        break;
                    case "ABM de cliente":
                        new BusquedaCli(this).Show(); //ver si depende del rol 
                        break;
                    case "ABM de empresa de espectaculos":
                        new BusquedaEmp(this).Show();
                        break;
                    case "ABM de categoria":
                        //falta pantallita de abm categorias
                        break;
                    case "ABM grado de publicacion":
                        // new GradoDeLaPublicacion(this).Show(); tiene que ser con empresa
                        break;
                    case " Generar publicacion":
                       // new CrearPublicacion(this).Show();
                        break;
                    case "Editar publicacion":
                       // new Editar_publicacion().Show();
                        break;
                    case "Comprar":
                        Cliente cliente = this.obtenerCliente();
                        new BuscarP(this).Show();
                        break;
                    case "Historial del cliente":
                        //new Historial().Show();
                        break;
                    case "Canje y administracion de puntos":
                        //new CanjePuntos().Show(); es para cliente
                        break;
                    case "Generar pago de comisiones":
                        new Rendicion().Show();
                        break;
                    case "Listado estadistico":
                        //new Listados().Show();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private Cliente obtenerCliente()
        {
            Cliente cliente = new Cliente();
            SqlDataReader reader = servidor.query("EXEC dbo.buscarClientePorUsername_sp '" + Sesion.sesion.usuario + "'");

            while (reader.Read())
            {
                cliente.Nombre = (reader["nombre"].ToString());
                cliente.Apellido = (reader["apellido"].ToString());
                cliente.TipoDocumento = (reader["tipo_documento"].ToString());
                cliente.NumeroDeDocumento = (Convert.ToInt32(reader["documento"]));
                cliente.Cuil = (Convert.ToInt32(reader["cuil"].ToString()));
                cliente.Mail = (reader["mail"].ToString());
                cliente.Telefono = (Convert.ToInt32(reader["telefono"].ToString()));
                cliente.FechaDeNacimiento = (Convert.ToDateTime(reader["fecha_creacion"].ToString()));
                cliente.Calle = (reader["calle"].ToString());
                cliente.NumeroDeCalle = (Convert.ToInt16(reader["numero_calle"].ToString()));
                cliente.Piso = (Convert.ToInt16(reader["piso"].ToString()));
                cliente.Departamento = (reader["depto"].ToString());
                cliente.CodigoPostal = (reader["codigo_postal"].ToString());
            }
            reader.Close();

            return cliente;
        }
    }
}
