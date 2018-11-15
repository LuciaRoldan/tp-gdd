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
    public partial class ResultadosBusquedaEmp : MiForm
    {
        public ResultadosBusquedaEmp(List<Empresa> resultados, MiForm anterior) : base(anterior)
        {
            InitializeComponent();
            var bindingList = new BindingList<Empresa>(resultados);
            var source = new BindingSource(bindingList, null);
            dataGridViewResultados.DataSource = source;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.cerrarAnteriores();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Anterior.Show();
            this.Close(); 
        }

        private void buttonModificar_Click(object sender, EventArgs e)
        {
            Empresa empresa = (Empresa)dataGridViewResultados.CurrentRow.DataBoundItem;
            this.Hide();
            new ModificarEmp(empresa, this).Show();
        }
    }
}
