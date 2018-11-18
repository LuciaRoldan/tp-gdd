using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PalcoNet.Dominio;

namespace PalcoNet.Registro_de_Usuario
{
    public partial class RegistroDomicilio : MiForm
    {
        Usuario usuario;

        public Usuario Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }

        public RegistroDomicilio(MiForm formAnterior, Usuario usuario) : base(formAnterior)
        {
            this.Usuario = usuario;
            InitializeComponent();
        }

        private bool camposCompletos()
        {
            return !string.IsNullOrWhiteSpace(textBoxCalle.Text)
                && !string.IsNullOrWhiteSpace(textBoxNro.Text)
                && !string.IsNullOrWhiteSpace(textBoxCiudad.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new SeleccionarFuncionalidad().Show();
            this.Hide();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
            this.Anterior.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Servidor servidor = Servidor.getInstance();
            if (this.camposCompletos())
            {
     
                    this.Usuario.Calle = textBoxCalle.Text;
                    this.Usuario.NumeroDeCalle = Int32.Parse(textBoxNro.Text);
                    this.Usuario.Ciudad = textBoxCiudad.Text;
                    if (!string.IsNullOrWhiteSpace(textBoxLocalidad.Text)) { this.Usuario.Localidad = textBoxLocalidad.Text; }
                    if (!string.IsNullOrWhiteSpace(textBoxPiso.Text)) { this.Usuario.Piso = Int32.Parse(textBoxPiso.Text); }
                    this.Usuario.Departamento = textBoxDepto.Text;
                    this.Usuario.CodigoPostal = textBoxCodigoPostal.Text;

                    if (this.Usuario is Empresa)
                    {
                            string query = "'" + this.Usuario.NombreUsuario + "', '" + this.Usuario.Contrasenia + "', '"
                            + ((Empresa)this.Usuario).RazonSocial + "', '" + ((Empresa)this.Usuario).Mail + "', '"
                            + ((Empresa)this.Usuario).Cuit + "', '" + this.Usuario.Calle + "','" + this.Usuario.NumeroDeCalle + "', '" + this.Usuario.Piso
                            + "', " + this.Usuario.Departamento + ", '" + Usuario.CodigoPostal + "'";

                        Console.WriteLine(query);

                        servidor.realizarQuery("EXEC dbo.registroEmpresa " + query);

                    }

                    string queryCli = "'" + this.Usuario.NombreUsuario + "', '" + this.Usuario.Contrasenia + "', '"
                            + ((Cliente)this.Usuario).Nombre + "', '" + ((Cliente)this.Usuario).Apellido + "', '"
                            + ((Cliente)this.Usuario).TipoDocumento + "', '" + ((Cliente)this.Usuario).NumeroDeDocumento + "', '"
                            + ((Cliente)this.Usuario).Cuil + "', '" + ((Cliente)this.Usuario).Mail + "', '" + ((Cliente)this.Usuario).Telefono + "', '" 
                            + this.Usuario.Calle + "','" + this.Usuario.NumeroDeCalle + "', '" + this.Usuario.Piso
                            + "', " + this.Usuario.Departamento + ", '" + Usuario.CodigoPostal + "'";

                    Console.WriteLine(queryCli);

                    servidor.realizarQuery("EXEC dbo.registroCliente " + queryCli);


                    this.Hide();
                    this.cerrarAnteriores();
                }

            
             
        }

        private void textBoxCiudad_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBoxCodigoPostal_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
