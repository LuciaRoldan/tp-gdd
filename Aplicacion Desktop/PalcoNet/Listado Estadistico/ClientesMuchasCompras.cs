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
    public partial class ClientesMuchasCompras : MiForm
    {
        public ClientesMuchasCompras(/*List<CompraCliente> comprasClientes,*/ MiForm formAnterior) : base(formAnterior)
        {/*
            InitializeComponent();
            var bindingList = new BindingList<CompraCliente>(comprasClientes);
            var source = new BindingSource(bindingList, null);
            clientesComprasGrid.DataSource = source;*/
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.cerrarAnteriores();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new Listados().Show();
            this.Hide(); 
        }
    }
}
