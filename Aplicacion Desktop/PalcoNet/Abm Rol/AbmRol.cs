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

namespace PalcoNet.Abm_Rol
{
    public partial class AbmRol : MiForm
    {
        List<Funcionalidad> funcionalidades = Enum.GetValues(typeof(Funcionalidad)).Cast<Funcionalidad>().ToList();

        List<Rol> roles = new List<Rol>();

        Rol rolSeleccionado;

        internal Rol RolSeleccionado
        {
            get { return rolSeleccionado; }
            set { rolSeleccionado = value; }
        }

        internal List<Rol> Roles
        {
            get { return roles; }
            set { roles = value; }
        }

        internal List<Funcionalidad> Funcionalidades
        {
            get { return funcionalidades; }
            set { funcionalidades = value; }
        }

        public AbmRol(MiForm anterior) : base(anterior)
        {
            InitializeComponent();
            foreach (Funcionalidad fun in this.Funcionalidades)
            {
                checkedListBoxFun2.Items.Add(fun);
                checkedListBoxFuncionalidades.Items.Add(fun);
            }

            //Aca hay que traer todos los roles de la base y guardarlos en la lista roles

            Rol rol1 = new Rol();
            Rol rol2 = new Rol();
            rol1.Nombre = "UNO";
            rol2.Nombre = "DOS";
            List<Funcionalidad> ff = new List<Funcionalidad>();
            ff.Add(Funcionalidad.Comprar);
            ff.Add(Funcionalidad.ABMRubro);
            List<Funcionalidad> fff = new List<Funcionalidad>();
            fff.Add(Funcionalidad.EditarPublicacion);
            fff.Add(Funcionalidad.VerHistorial);
            rol1.Funcionalidades = ff;
            rol2.Funcionalidades = fff;

            this.Roles.Add(rol1);
            this.Roles.Add(rol2);

            foreach (Rol rol in this.Roles)
            {
                comboBoxRoles.Items.Add(rol.Nombre);
            }

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.cerrarAnteriores();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.cerrarAnteriores();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Capaz estaria bueno que salga un cartelito de que salio todo bien
            if (!string.IsNullOrWhiteSpace(textBoxNombre.Text) && checkedListBoxFuncionalidades.CheckedIndices.Count > 0)
            {
                string Nombre = textBoxNombre.Text;
                List<Funcionalidad> funcionalidadesSeleccionadas = new List<Funcionalidad>();
                foreach(Funcionalidad f in checkedListBoxFuncionalidades.CheckedItems){
                    funcionalidadesSeleccionadas.Add(f);
                }

                //Aca hay que guardar una nueva funcionalidad en la base con el nombre del rol y las funcionalidades

                MessageBox.Show("Se creó el rol " + Nombre + " de forma exitosa.", "Rol creado", MessageBoxButtons.OK);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Aca hay que hacer que se guarden los cambios en las tablas
            //Capaz estaria bueno que salga un cartelito de que salio todo bien

            if (!string.IsNullOrWhiteSpace(this.textBoxNomb.Text) && checkedListBoxFun2.SelectedItems.Count > 0)
            {
                Rol rolModificado = new Rol();
                rolModificado.Nombre = this.textBoxNomb.Text;
                List<Funcionalidad> funcionalidadesSeleccionadas = new List<Funcionalidad>();
                foreach (Funcionalidad f in checkedListBoxFuncionalidades.CheckedItems)
                {
                    funcionalidadesSeleccionadas.Add(f);
                }
                rolModificado.Funcionalidades = funcionalidadesSeleccionadas;

                //Aca hay que actualizar los datos en la base

                MessageBox.Show("Se actualizó el rol de forma exitosa.", "Rol editado", MessageBoxButtons.OK);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.checkedListBoxFun2.Enabled = true;
            this.RolSeleccionado = this.Roles[comboBoxRoles.SelectedIndex];
            for (int i = 0; this.checkedListBoxFun2.Items.Count > i; i++) {
                if (this.RolSeleccionado.Funcionalidades.Contains(this.Funcionalidades[i])){
                    this.checkedListBoxFun2.SetItemChecked(i, true);
                } else {
                    this.checkedListBoxFun2.SetItemChecked(i, false);
                }
            }
            this.textBoxNomb.Text = this.RolSeleccionado.Nombre;
        }
    }
}
