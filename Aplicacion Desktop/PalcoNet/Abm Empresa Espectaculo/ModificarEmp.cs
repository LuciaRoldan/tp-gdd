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
using System.Data.SqlClient;

namespace PalcoNet.Abm_Empresa_Espectaculo
{
    public partial class ModificarEmp : MiForm
    {
        bool fueModificada = false;
        Servidor servidor = Servidor.getInstance();
        Empresa empresaVieja;
        Int32 numeroCalle;
        Int32 piso;
        String depto;
        String codigoPostal;
        SqlDataReader readerEmpresa;

        public bool FueModificada
        {
            get { return fueModificada; }
            set { fueModificada = value; }
        }

        public ModificarEmp(Empresa empresa, MiForm anterior) : base(anterior)
        {
            InitializeComponent();

            textBoxRazonSocial.Text += empresa.RazonSocial;
            textBoxMail.Text += empresa.Mail;
            var cuit = empresa.Cuit;
            textBoxCuit.Text += cuit == 0 ? null : cuit.ToString();
   
            empresaVieja = empresa;

            readerEmpresa = servidor.query("EXEC MATE_LAVADO.obtenerDatosAdicionalesEmpresa '" + empresa.Id + "'");

            readerEmpresa.Read();

            readerEmpresa.Close();

            textBoxCalle.Text += empresaVieja.Calle;
            textBoxNumeroCalle.Text += empresaVieja.NumeroDeCalle == 0 ? null : empresaVieja.NumeroDeCalle.ToString();
            textBoxPiso.Text += empresaVieja.Piso == 0 ? null : empresaVieja.Piso.ToString();
            textBoxDepto.Text += empresaVieja.Departamento;
            textBoxCodigoPostal.Text += empresaVieja.CodigoPostal;
            textBoxCiudad.Text += empresaVieja.Ciudad;
            textBoxLocalidad.Text += empresaVieja.Localidad;
            
            
        }

        public bool verificarCampos()
        {
            string errores = "";
            long numero;
            bool camposCompletos = !string.IsNullOrWhiteSpace(textBoxMail.Text)
                && !string.IsNullOrWhiteSpace(textBoxCuit.Text)
                && !string.IsNullOrWhiteSpace(textBoxRazonSocial.Text);

            if (!camposCompletos) {
                errores += "Todos los campos deben estar completos.";
            } else {
                if (!long.TryParse(textBoxCuit.Text, out numero)) { errores += "El CUIT debe ser un valor numérico. \n"; }
            }

            if (!long.TryParse(textBoxCuit.Text, out numero))
            {
                //Verificamos que el CUIT tenga el largo que corresponde
                if (!(Int64.Parse(textBoxCuit.Text) > 9999999999 & Int64.Parse(textBoxCuit.Text) < 100000000000))
                { errores += "El CUIL debe poseer 11 digitos. \n"; }
                else
                {
                    //Verificamos que el CUIL sea valido

                    Servidor servidor = Servidor.getInstance();
                    string query = "'" + Int64.Parse(textBoxCuit.Text) + "'";
                    SqlDataReader reader = servidor.query("EXEC MATE_LAVADO.cuilEsValido_sp " + query);

                    while (reader.Read())
                    {
                        if (!bool.Parse(reader["valido"].ToString()))
                        {
                            errores += "Ingrese un CUIL válido. \n";
                        }
                    }
                }

            }
            


            if (errores != "") {
                Console.WriteLine("Hay errores");
                MessageBox.Show(errores, "Error", MessageBoxButtons.OK);
                return false;
            }
            return true;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Anterior.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.verificarCampos() && this.FueModificada) //Tambien faltaria verificar que no sean nulos los ingresados
            {
                Empresa empresaModificada = new Empresa();
                empresaModificada.RazonSocial = textBoxRazonSocial.Text;
                empresaModificada.Cuit = Int64.Parse(textBoxCuit.Text);
                empresaModificada.Mail = textBoxMail.Text;
                empresaModificada.Calle = textBoxCalle.Text;
                numeroCalle = Int32.Parse(textBoxNumeroCalle.Text);
                if (!string.IsNullOrWhiteSpace(textBoxPiso.Text)) { piso = Int32.Parse(textBoxPiso.Text); }
                if (!string.IsNullOrWhiteSpace(textBoxDepto.Text)) { depto = textBoxDepto.Text; }
                codigoPostal = textBoxCodigoPostal.Text;
                empresaModificada.Ciudad = textBoxCiudad.Text;
                empresaModificada.Localidad = textBoxLocalidad.Text;
                //Aca hay que hacer el update en la base

                String query = empresaVieja.Id + "', '" + empresaModificada.RazonSocial + "', '" + empresaModificada.Mail
                                + "', '" + empresaModificada.Cuit + "', '"
                                + empresaModificada.Calle + "', " + numeroCalle + ", " + piso + ", '" + depto + "', '" + codigoPostal + "', '" +
                                empresaModificada.Localidad + "', '" + empresaModificada.Ciudad + "'";
                servidor.realizarQuery("EXEC MATE_LAVADO.modificarEmpresa_sp '" + query);
                MessageBox.Show("Los cambios se realizaron exitosamente.", "Modificar empresa", MessageBoxButtons.OK);

                new SeleccionarFuncionalidad().Show();
                this.Close();
            }
            
        }

        private void textBoxRazonSocial_TextChanged(object sender, EventArgs e)
        {
            this.FueModificada = true;
        }

        private void textBoxMail_TextChanged(object sender, EventArgs e)
        {
            this.FueModificada = true;
        }

        private void textBoxCuit_TextChanged(object sender, EventArgs e)
        {
            this.FueModificada = true;
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void ModificarEmp_Load(object sender, EventArgs e)
        {

        }
    }
}
