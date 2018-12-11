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

namespace PalcoNet.Comprar
{
    public partial class Espectaculo : MiForm
    {
        Compra compra;
        List<DateTime> fechas = new List<DateTime>();

        public List<DateTime> Fechas
        {
            get { return fechas; }
            set { fechas = value; }
        }

        public Compra Compra
        {
            get { return compra; }
            set { compra = value; }
        }

        public Espectaculo(MiForm anterior, Compra compra)
        {
            InitializeComponent();
            this.Compra = compra;

            Servidor servidor = Servidor.getInstance();
            SqlDataReader reader = servidor.query("exec buscarEspectaculosPorPublicacion_sp " + compra.Publicacion.Id);
            while (reader.Read()) {
                DateTime fecha = (DateTime)reader["fecha_evento"];
                comboBoxFechas.Items.Add(fecha);
                this.fechas.Add(fecha);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBoxFechas.SelectedIndex > -1)
            {
                this.Compra.Fecha = this.fechas[comboBoxFechas.SelectedIndex];
                new Ubicaciones(compra, this).Show();
                this.Hide();
            }
        }
    }
}
