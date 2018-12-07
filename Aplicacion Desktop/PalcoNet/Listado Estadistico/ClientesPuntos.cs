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

namespace PalcoNet.Listado_Estadistico
{
    public partial class ClientesPuntos : MiForm
    {
        public ClientesPuntos(List<Cliente> resultados, MiForm formAnterior) : base(formAnterior)
        {
            InitializeComponent();
            var bindingList = new BindingList<Cliente>(resultados);
            var source = new BindingSource(bindingList, null);
            puntosClientesGrid.DataSource = source;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.cerrarAnteriores();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Anterior.Show();
            this.Hide(); 
        }

        private void puntosClientesGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
