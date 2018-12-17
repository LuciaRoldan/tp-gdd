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
            //String lol = comboBox1.Text.substring(0, Math.Min(3, comboBox1.Text.Length));

            switch (rol)
            {
                case "Cli":
                    
                    Cliente cli = new Cliente();
                    cli.IdUsuario = sesion.usuario.IdUsuario;

                    SqlDataReader reader = servidor.query("EXEC MATE_LAVADO.elClienteExiste_sp '" + sesion.usuario.IdUsuario + "'");
                    reader.Read();

                    if (!bool.Parse(reader["existe_el_cliente"].ToString()))
                    {
                        sesion.rol.Nombre = "Cliente";
                        Console.Write("el cliente no existe");
                        new RegistroDeCliente(cli, this).Show();
                        //this.Hide();
                        break;
                    }

                    SqlDataReader reader2 = servidor.query("EXEC MATE_LAVADO.elClienteTieneInfoCompleta_sp '" + sesion.usuario.IdUsuario + "'");
                            reader2.Read();
                            if (bool.Parse(reader2["esta_completa"].ToString()))
                            {
                                sesion.rol.Nombre = "Cliente";
                                new SeleccionarFuncionalidad().Show();
                                //this.Hide();
                                break;
                            }
                            else
                            {
                                sesion.rol.Nombre = "Cliente";
                                Console.Write("el cliente esta incompleto");
                                cli = Sesion.getInstance().traerCliente();
                                new ModificarCli(cli, this).Show();
                                //this.Hide();
                                break;
                            }

                case "Emp":
                    Empresa emp = new Empresa();
                    emp.IdUsuario = sesion.usuario.IdUsuario;

                    SqlDataReader reader3 = servidor.query("EXEC MATE_LAVADO.laEmpresaExiste_sp '" + sesion.usuario.IdUsuario + "'");
                    reader3.Read();
                    
                    //Console.Write(bool.Parse(reader3["existe_la_empresa"].ToString()));
                    if (!bool.Parse(reader3["existe_la_empresa"].ToString()))
                    {
                        sesion.rol.Nombre = "Empresa";
                        new RegistroDeEmpresa(emp, this).Show();
                        //this.Hide();
                        break;
                    }

                    SqlDataReader reader4 = servidor.query("EXEC MATE_LAVADO.laEmpresaTieneInfoCompleta_sp '" + sesion.usuario.IdUsuario + "'");
                    reader4.Read();
                    if (bool.Parse(reader4["esta_completa"].ToString()))
                    {
                        sesion.rol.Nombre = "Empresa";
                        new SeleccionarFuncionalidad().Show();
                        //this.Hide();
                        break;
                    }
                    else
                    {
                        sesion.rol.Nombre = "Empresa";
                        emp = Sesion.getInstance().traerEmpresa();
                        new ModificarEmp(emp, this).Show();
                        //this.Hide();
                        break;
                    }
                
                default:
                    new SeleccionarFuncionalidad().Show();
                    break;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            sesion.rol.Nombre = comboBox1.Text;
        }
    }
}
