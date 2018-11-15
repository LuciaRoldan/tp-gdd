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
        Empresa empresa;

        public Empresa Empresa
        {
            get { return empresa; }
            set { empresa = value; }
        }

        public ModificarEmp(Empresa empresa, MiForm anterior) : base(anterior)
        {
            this.empresa = empresa;
            InitializeComponent();
        }

        public bool empresaNoFueModifiada(Empresa empresaModificada)
        {
            /* return empresa.RazonSocial.Equals(empresaModificada.RazonSocial)
                 && empresa.Cuit.Equals(empresaModificada.Cuit)
                 && empresa.Mail.Equals(empresaModificada.Mail)
                 && empresa.Telefono.Equals(empresaModificada.Telefono);*/
            return true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Anterior.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Empresa empresaModificada = new Empresa();
            empresaModificada.RazonSocial = textBoxRazonSocial.Text;
            empresaModificada.Cuit = Int32.Parse(textBoxCuit.Text);
            empresaModificada.Mail = textBoxMail.Text;
            empresaModificada.Telefono = Int32.Parse(textBoxTelefono.Text);

            if (!this.empresaNoFueModifiada(empresaModificada))
            {
                //Aca hay que hacer el update en la base
            }

            new BusquedaEmp(this.Anterior.Anterior.Anterior).Show();
            this.Close();
        }
    }
}
