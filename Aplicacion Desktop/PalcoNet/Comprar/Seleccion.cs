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


namespace PalcoNet.Comprar
{
    public partial class Seleccion : MiForm
    {
        string descripcion;

        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }
        DateTime? desde;

        public DateTime? Desde
        {
            get { return desde; }
            set { desde = value; }
        }
        DateTime? hasta;

        public DateTime? Hasta
        {
            get { return hasta; }
            set { hasta = value; }
        }
        List<string> categorias;

        public List<string> Categorias
        {
            get { return categorias; }
            set { categorias = value; }
        }

        List<Publicacion> resultados = new List<Publicacion>();

        internal List<Publicacion> Resultados
        {
            get { return resultados; }
            set { resultados = value; }
        }


        public Seleccion(string descripcion, DateTime? desde, DateTime? hasta, List<string> categorias ,MiForm anterior) : base(anterior)
        {
            this.Descripcion = descripcion;
            this.Desde = desde;
            this.Hasta = hasta;
            this.Categorias = categorias;
            InitializeComponent();

            //Aca hay que buscar los primeros resultados. Cuando se busca hay que verificar que no sean null las cosas y guardar los resultados en resultados

            var bindingList = new BindingList<Publicacion>(resultados);
            var source = new BindingSource(bindingList, null);
            dataGridViewResultados.DataSource = source;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Anterior.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Aca hay que buscar los siguientes elementos en la base y reemplazar el contenido de la lista a la que esta bindeada la tabla
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Publicacion publicacionSeleccionada = (Publicacion)dataGridViewResultados.CurrentRow.DataBoundItem;
            Compra compra = new Compra();
            compra.Publicacion = publicacionSeleccionada;
            new Ubicaciones(compra, this).Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Aca hay que buscar los anteriores elementos en la base y reemplazar el contenido de la lista a la que esta bindeada la tabla
        }
    }
}
