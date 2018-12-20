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
    public partial class Espectaculos : MiForm
    {
        Compra compra;
        List<Espectaculo> fechas = new List<Espectaculo>();

        public List<Espectaculo> Fechas
        {
            get { return fechas; }
            set { fechas = value; }
        }

        public Compra Compra
        {
            get { return compra; }
            set { compra = value; }
        }

        public Espectaculos(MiForm anterior, Compra compra) : base(anterior)
        {
            InitializeComponent();
            this.Compra = compra;

            //Segun la publicacion seleccionada busca sus fechas disponibles y las muestra para que el usuario pueda elegir cual comprar

            Servidor servidor = Servidor.getInstance();
            SqlDataReader reader = servidor.query("exec MATE_LAVADO.buscarEspectaculosPorPublicacion_sp " + compra.Publicacion.Id);
            while (reader.Read()) {
                Espectaculo e = new Espectaculo();
                e.Fecha = (DateTime)reader["fecha_evento"];
                e.Id = int.Parse(reader["id_espectaculo"].ToString());
                comboBoxFechas.Items.Add(e.Fecha);
                this.fechas.Add(e);
            }
        }

        //Una vez elegida la fecha el siguiente paso será elegir las ubicaciones
        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBoxFechas.SelectedIndex > -1)
            {
                this.Compra.Espectaculo = this.fechas[comboBoxFechas.SelectedIndex];
                new Ubicaciones(compra, this).Show();
                this.Hide();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Anterior.Show();
            this.Hide();
        }
    }
}
