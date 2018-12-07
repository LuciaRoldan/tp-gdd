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

        public bool FueModificada
        {
            get { return fueModificada; }
            set { fueModificada = value; }
        }

        public ModificarEmp(Empresa empresa, MiForm anterior) : base(anterior)
        {
            InitializeComponent();
        }

        public bool verificarCampos()
        {
            string errores = "";
            int numero;
            bool camposCompletos = !string.IsNullOrWhiteSpace(textBoxTelefono.Text)
                && !string.IsNullOrWhiteSpace(textBoxMail.Text)
                && !string.IsNullOrWhiteSpace(textBoxCuit.Text)
                && !string.IsNullOrWhiteSpace(textBoxRazonSocial.Text);

            if (!camposCompletos) {
                errores += "Todos los campos deben estar completos.";
            } else {
                if (!int.TryParse(textBoxCuit.Text, out numero)) { errores += "El CUIT debe ser un valor numérico. \n"; }
                if (!int.TryParse(textBoxTelefono.Text, out numero)) { errores += "El teléfono debe ser un valor numérico. \n"; }
            }

            if (errores != "") { 
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
                empresaModificada.Cuit = Int32.Parse(textBoxCuit.Text);
                empresaModificada.Mail = textBoxMail.Text;
                empresaModificada.Telefono = Int32.Parse(textBoxTelefono.Text);
                //Aca hay que hacer el update en la base

                MessageBox.Show("Los cambios se realizaron exitosamente.", "Modificar cliente", MessageBoxButtons.OK);
                this.cerrarAnteriores();
            }
            
        }
    }
}
