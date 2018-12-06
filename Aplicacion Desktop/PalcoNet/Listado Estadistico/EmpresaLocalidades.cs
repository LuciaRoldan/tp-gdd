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
    public partial class EmpresaLocalidades : MiForm
    {

        DateTime fechaInicio;
        DateTime fechaFin;

        public EmpresaLocalidades(DateTime inicio, DateTime fin, MiForm formAnterior) : base(formAnterior) 
        {
            InitializeComponent();
            fechaInicio = inicio;
            fechaFin = fin;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.cerrarAnteriores();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Hace que aparezcan los datos abajo
            //Le pasa una consulta a la BD con la fecha inicio, fecha fin, grado de visibilidad, mes, anio.
            //Ordenar por fecha y luego por visibilidad.
            //Devuelve Empresa, CUIT, CantidadLocalidadesNoVendidas

            List<CantidadLocalidadesEmpresa> localidadesEmpresa = new List<CantidadLocalidadesEmpresa>();
            
            var bindingList = new BindingList<CantidadLocalidadesEmpresa>(localidadesEmpresa);
            var source = new BindingSource(bindingList, null);
            localidadesEmpresaGrid.DataSource = source;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Anterior.Show();
            this.Hide(); 
        }
    }
}
