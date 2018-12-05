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

namespace PalcoNet.Abm_Cliente
{
    public partial class ResultadoBusquedaCli : MiForm
    {

        public ResultadoBusquedaCli(List<Cliente> resultados, MiForm formAnterior) : base(formAnterior)
        {
            InitializeComponent();
            var bindingList = new BindingList<Cliente>(resultados);
            var source = new BindingSource(bindingList, null);
            dataGridResultados.DataSource = source;
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void buttonModificar_Click(object sender, EventArgs e)
        {
            Cliente cliente = (Cliente)dataGridResultados.CurrentRow.DataBoundItem;
            this.Hide();
            new ModificarCli(cliente, this).Show();
        }

        private void ResultadoBusquedaCli_Load(object sender, EventArgs e)
        {

        }
    }
}
