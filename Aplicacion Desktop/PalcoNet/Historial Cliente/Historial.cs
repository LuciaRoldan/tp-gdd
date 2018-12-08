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

namespace PalcoNet.Historial_Cliente
{
    public partial class Historial : MiForm
    {
        public Historial(MiForm anterior) : base(anterior)
        {
            if (Sesion.getInstance().rol.Nombre == "Cliente")
            {
                Cliente cliente = (Cliente)Sesion.getInstance().usuario;
                
                //Aca hay que buscar los primeros n elementos de la paginacion y guardarlos en la lista de abajo
                //La query tiene que devolver descripcion, fecha, importe y cantidad de asientos de cada compra del cliente
                List<ElementoHistorialCliente> historial = new List<ElementoHistorialCliente>();
                var bindingList = new BindingList<ElementoHistorialCliente>(historial);
                var source = new BindingSource(bindingList, null);
                tabla.DataSource = source;
            }
            else 
            {
                MessageBox.Show("Se encuentra loggeado como " + Sesion.getInstance().rol.Nombre + " por lo cual no podrá utilizar esta funcionalidad.", "Advertencia", MessageBoxButtons.OK);
            }

            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.cerrarAnteriores();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Aca hay que buscar los siguientes n elementos de la paginacion y guardarlos en la lista de abajo
            List<ElementoHistorialCliente> historial = new List<ElementoHistorialCliente>();
            var bindingList = new BindingList<ElementoHistorialCliente>(historial);
            var source = new BindingSource(bindingList, null);
            tabla.DataSource = source;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Aca hay que buscar los anteriores n elementos de la paginacion y guardarlos en la lista de abajo
            List<ElementoHistorialCliente> historial = new List<ElementoHistorialCliente>();
            var bindingList = new BindingList<ElementoHistorialCliente>(historial);
            var source = new BindingSource(bindingList, null);
            tabla.DataSource = source;
        }
    }
}
