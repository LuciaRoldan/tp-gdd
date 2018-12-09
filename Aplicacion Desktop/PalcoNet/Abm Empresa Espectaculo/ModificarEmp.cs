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

namespace PalcoNet.Abm_Empresa_Espectaculo
{
    public partial class ModificarEmp : MiForm
    {
        bool fueModificada = false;
        Servidor servidor = Servidor.getInstance();
        Empresa empresaVieja;

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
            textBoxCuit.Text += empresa.Cuit;
   
            empresaVieja = empresa;
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
                Console.WriteLine("Estoy en el if");
                Empresa empresaModificada = new Empresa();
                empresaModificada.RazonSocial = textBoxRazonSocial.Text;
                empresaModificada.Cuit = Int64.Parse(textBoxCuit.Text);
                empresaModificada.Mail = textBoxMail.Text; 
                //Aca hay que hacer el update en la base

                String query = empresaVieja.Cuit + "', '" + empresaModificada.RazonSocial + "', '" + empresaModificada.Mail
                                + "', '" + empresaModificada.Cuit + "'";
                servidor.realizarQuery("EXEC modificarEmpresa_sp '" + query);
                MessageBox.Show("Los cambios se realizaron exitosamente.", "Modificar cliente", MessageBoxButtons.OK);
                this.cerrarAnteriores();
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
    }
}
