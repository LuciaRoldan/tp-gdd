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
            sesion.rol.Nombre = comboBox1.Text.ToString();

            switch (sesion.rol.Nombre)
            {
                case "Cliente":
                    Cliente cli = new Cliente();
                    SqlDataReader reader = servidor.query("EXEC MATE_LAVADO.elClienteExiste_sp '" + sesion.usuario.NombreUsuario + "'");
                    reader.Read();
                    if (!(int.Parse(reader["existe_el_cliente"].ToString()) == 1))
                    {
                        new RegistroDeCliente(cli, this).Show();
                        this.Hide();
                    }

                    SqlDataReader reader2 = servidor.query("EXEC MATE_LAVADO.elClienteTieneInfoCompleta_sp '" + sesion.usuario.NombreUsuario + "'");
                    reader2.Read();
                    if (int.Parse(reader2["esta_completa"].ToString()) == 1)
                    {
                        new SeleccionarFuncionalidad().Show();
                    }
                    else
                    {
                        cli = Sesion.getInstance().traerCliente();
                        new ModificarCli(cli, this).Show();
                    }
                    break;
                    
                case "Empresa":
                    Empresa emp = new Empresa();
                    SqlDataReader reader3 = servidor.query("EXEC MATE_LAVADO.laEmpresaExiste_sp '" + sesion.usuario.NombreUsuario + "'");
                    reader3.Read();
                    if (!(int.Parse(reader3["existe_la_empresa"].ToString()) == 1))
                    {
                        new RegistroDeEmpresa(emp, this).Show();
                        this.Hide();
                    }

                    SqlDataReader reader4 = servidor.query("EXEC MATE_LAVADO.laEmpresaTieneInfoCompleta_sp '" + sesion.usuario.NombreUsuario + "'");
                    reader4.Read();
                    if (int.Parse(reader4["esta_completa"].ToString()) == 1)
                    {
                        new SeleccionarFuncionalidad().Show();
                    }
                    else
                    {
                        emp = Sesion.getInstance().traerEmpresa();
                        new ModificarEmp(emp, this).Show();
                    }
                    break;
                default:
                    new SeleccionarFuncionalidad().Show();
                    break;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
