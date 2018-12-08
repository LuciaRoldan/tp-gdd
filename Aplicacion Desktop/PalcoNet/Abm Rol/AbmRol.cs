using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
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
        List<String> funcionalidades = new List<String>();
        List<String> funcionalidadesSeleccionadas = new List<String>();
        List<Rol> roles = new List<Rol>();

        Rol rolSeleccionado = new Rol();
        Servidor servidor = Servidor.getInstance();

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

        internal List<String> Funcionalidades
        {
            get { return funcionalidades; }
            set { funcionalidades = value; }
        }

        public AbmRol(MiForm anterior) : base(anterior)
        {
            InitializeComponent();
  
                SqlDataReader reader = servidor.query("SELECT DISTINCT nombre FROM Funcionalidades");

                while (reader.Read())
                {
                    checkedListBoxFuncionalidades.Items.Add(reader["nombre"].ToString());
                    this.Funcionalidades.Add(reader["nombre"].ToString());
                    checkedListBoxFun2.Items.Add(reader["nombre"].ToString());
                }
                reader.Close();

                reader = servidor.query("SELECT DISTINCT nombre FROM Roles");

                while (reader.Read())
                {
                    comboBoxRoles.Items.Add(reader["nombre"].ToString());
                }
                reader.Close();
            
            //Aca hay que traer todos los roles de la base y guardarlos en la lista roles

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
            this.cerrarAnteriores();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Capaz estaria bueno que salga un cartelito de que salio todo bien
            if (!string.IsNullOrWhiteSpace(textBoxNombre.Text) && checkedListBoxFuncionalidades.CheckedIndices.Count > 0)
            {
                string nombre = textBoxNombre.Text;
                servidor.realizarQuery("EXEC dbo.agregarRol_sp '" + nombre + "'");
                foreach(String f in checkedListBoxFuncionalidades.CheckedItems){
                    funcionalidadesSeleccionadas.Add(f);
                }
                foreach (String fun in funcionalidadesSeleccionadas)
                {
                    servidor.realizarQuery("EXEC dbo.AgregarFuncionalidadARol_sp '" + nombre + "', '" + fun + "'");
                }

                //Aca hay que guardar una nueva funcionalidad en la base con el nombre del rol y las funcionalidades

                for (int i = 0; this.checkedListBoxFuncionalidades.Items.Count > i; i++)
                {
                    this.checkedListBoxFuncionalidades.SetItemChecked(i, false);
                }
                textBoxNombre.ResetText();

                MessageBox.Show("Se creó el rol " + nombre + " de forma exitosa.", "Rol creado", MessageBoxButtons.OK);
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
                Console.WriteLine(rolSeleccionado.Nombre);

                servidor.realizarQuery("EXEC dbo.eliminarFuncionalidadesRol_sp '" + rolSeleccionado.Nombre + "'");
                               
                foreach (String f in checkedListBoxFun2.CheckedItems)
                {
                    funcionalidadesSeleccionadas.Add(f);
                }
                foreach (String fun in funcionalidadesSeleccionadas)
                {
                    servidor.realizarQuery("EXEC dbo.AgregarFuncionalidadARol_sp '" + rolSeleccionado.Nombre + "', '" + fun + "'");
                }

                servidor.realizarQuery("EXEC dbo.modificarNombreRol_sp '" + rolSeleccionado.Nombre + "' , '" + rolModificado.Nombre + "'");
                rolSeleccionado = rolModificado;
               
                //Aca hay que actualizar los datos en la base

                for (int i = 0; this.checkedListBoxFun2.Items.Count > i; i++)
                {
                     this.checkedListBoxFun2.SetItemChecked(i, false);
                }
                textBoxNomb.ResetText();
                comboBoxRoles.ResetText();

                MessageBox.Show("Se actualizó el rol de forma exitosa.", "Rol editado", MessageBoxButtons.OK);

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            rolSeleccionado.Nombre = comboBoxRoles.SelectedItem.ToString();
            this.checkedListBoxFun2.Enabled = true;
            this.button5.Enabled = true;
            this.button6.Enabled = true;

            SqlDataReader reader = servidor.query("EXEC dbo.getFuncionalidadesDeRol_sp '" + rolSeleccionado.Nombre + "'");


            while (reader.Read())
            {
                String funcionalidadSeleccionada;
                funcionalidadSeleccionada = reader["nombre"].ToString();
                funcionalidadesSeleccionadas.Add(funcionalidadSeleccionada);
                
            }
            reader.Close();
            
            for (int i = 0; this.checkedListBoxFun2.Items.Count > i; i++) {
                if (funcionalidadesSeleccionadas.Contains(this.Funcionalidades[i])){
                    this.checkedListBoxFun2.SetItemChecked(i, true);
                } else {
                    this.checkedListBoxFun2.SetItemChecked(i, false);
                }
            }
            this.textBoxNomb.Text = this.rolSeleccionado.Nombre;
            funcionalidadesSeleccionadas.Clear();
        }

      
        private void checkedListBoxFuncionalidades_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            servidor.realizarQuery("EXEC dbo.modificarRol_sp '" + rolSeleccionado.Nombre + "'," + 0);

            MessageBox.Show("El Rol ha sido inhabilitado.", "Rol inhabilitado", MessageBoxButtons.OK);

            for (int i = 0; this.checkedListBoxFun2.Items.Count > i; i++)
            {
                this.checkedListBoxFun2.SetItemChecked(i, false);
            }
            textBoxNomb.ResetText();
            comboBoxRoles.ResetText();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (/*aca faltaria verificar si esta deshabilitado*/ true){
                    servidor.realizarQuery("EXEC dbo.modificarRol_sp '" + rolSeleccionado.Nombre + "'," + 1);

                    MessageBox.Show("El Rol ha sido habilitado.", "Rol habilitado", MessageBoxButtons.OK);

                    for (int i = 0; this.checkedListBoxFun2.Items.Count > i; i++)
                    {
                        this.checkedListBoxFun2.SetItemChecked(i, false);
                    }
                    textBoxNomb.ResetText();
                    comboBoxRoles.ResetText();
            }
        }
    }
}
