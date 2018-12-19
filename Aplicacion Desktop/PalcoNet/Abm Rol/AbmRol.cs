using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
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
  
                SqlDataReader reader = servidor.query("SELECT DISTINCT nombre FROM MATE_LAVADO.Funcionalidades");

                while (reader.Read())
                {
                    checkedListBoxFuncionalidades.Items.Add(reader["nombre"].ToString());
                    this.Funcionalidades.Add(reader["nombre"].ToString());
                    checkedListBoxFun2.Items.Add(reader["nombre"].ToString());
                }
                reader.Close();

                //Aca traemos todos las funcionalidades de la base y las mostramos en el checkedList 
                
               reader = servidor.query("SELECT DISTINCT nombre FROM MATE_LAVADO.Roles");

                while (reader.Read())
                {
                    comboBoxRoles.Items.Add(reader["nombre"].ToString());
                }
                reader.Close();

                this.button5.Enabled = false;
                this.button6.Enabled = false;
                this.button7.Enabled = false;
            
            //Aca traemos todos los roles de la basey los mostramos en el comboBox

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
            if (!string.IsNullOrWhiteSpace(textBoxNombre.Text) && checkedListBoxFuncionalidades.CheckedIndices.Count > 0)
            {
                string nombre = textBoxNombre.Text;
                servidor.realizarQuery("EXEC MATE_LAVADO.agregarRol_sp '" + nombre + "'");
                foreach(String f in checkedListBoxFuncionalidades.CheckedItems){
                    funcionalidadesSeleccionadas.Add(f);
                }
                foreach (String fun in funcionalidadesSeleccionadas)
                {
                    servidor.realizarQuery("EXEC MATE_LAVADO.AgregarFuncionalidadARol_sp '" + nombre + "', '" + fun + "'");
                }

                //Aca creamos el ROL, lo guardamos en la base y lo asociamos a las funcionalidades elegidas

                for (int i = 0; this.checkedListBoxFuncionalidades.Items.Count > i; i++)
                {
                    this.checkedListBoxFuncionalidades.SetItemChecked(i, false);
                }
                textBoxNombre.ResetText();

                MessageBox.Show("Se creó el rol " + nombre + " de forma exitosa.", "Rol creado", MessageBoxButtons.OK);

                

               SqlDataReader reader = servidor.query("SELECT DISTINCT nombre FROM MATE_LAVADO.Roles");
               comboBoxRoles.Items.Clear();
                while (reader.Read())
                {
                    comboBoxRoles.Items.Add(reader["nombre"].ToString());
                }
                reader.Close();
                
                
            
            } 
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(this.textBoxNomb.Text) && checkedListBoxFun2.SelectedItems.Count > 0)
            {

                Rol rolModificado = new Rol();
                rolModificado.Nombre = this.textBoxNomb.Text;
                Console.WriteLine(rolSeleccionado.Nombre);

                servidor.realizarQuery("EXEC MATE_LAVADO.eliminarFuncionalidadesRol_sp '" + rolSeleccionado.Nombre + "'");

                //eliminamos todas las funcionalidades  y recuperamos las que habian sido seleccionadas para mostrarlas 
                //como elegidas. La persona marca o desmarca las que quiera, las relacionamos con el rol, tambien podemos
                // modificar el nombre, obtenemos el nuevo texto y actualizamos todo

                foreach (String f in checkedListBoxFun2.CheckedItems)
                {
                    funcionalidadesSeleccionadas.Add(f);
                }
                foreach (String fun in funcionalidadesSeleccionadas)
                {
                    servidor.realizarQuery("EXEC MATE_LAVADO.AgregarFuncionalidadARol_sp '" + rolSeleccionado.Nombre + "', '" + fun + "'");
                }

                servidor.realizarQuery("EXEC MATE_LAVADO.modificarNombreRol_sp '" + rolSeleccionado.Nombre + "' , '" + rolModificado.Nombre + "'");
                rolSeleccionado = rolModificado;

                for (int i = 0; i < this.checkedListBoxFun2.Items.Count; i++)
                {
                    //Console.WriteLine(this.checkedListBoxFun2.GetItemCheckState(i));
                    this.checkedListBoxFun2.SetItemChecked(i, false);
                    this.checkedListBoxFun2.SetItemCheckState(i, CheckState.Unchecked);
                    Console.WriteLine(this.checkedListBoxFun2.GetItemCheckState(i));
                }
                textBoxNomb.ResetText();
                comboBoxRoles.SelectedIndex = -1;
                this.button5.Enabled = false;
                this.button6.Enabled = false;
                this.button7.Enabled = false;
                this.checkedListBoxFun2.Enabled = false;

                MessageBox.Show("Se actualizó el rol de forma exitosa.", "Rol editado", MessageBoxButtons.OK);

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //mientras no haya rol seleccionado no estarán habilitados los demás campos

            rolSeleccionado.Nombre = comboBoxRoles.Text;
            this.checkedListBoxFun2.Enabled = true;
            this.button5.Enabled = true;
            this.button6.Enabled = true;
            this.button7.Enabled = true;

            SqlDataReader reader = servidor.query("EXEC MATE_LAVADO.getFuncionalidadesDeRol_sp '" + rolSeleccionado.Nombre + "'");
            //selecciona en el checkedList las funcionalidades que tenia el rol originalmente 

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

        private void button5_Click_1(object sender, EventArgs e) //inhabilita el rol pasando el bit habilitado a 0
        {
            servidor.realizarQuery("EXEC MATE_LAVADO.modificarRol_sp '" + rolSeleccionado.Nombre + "'," + 0);

            MessageBox.Show("El Rol está inhabilitado.", "Rol inhabilitado", MessageBoxButtons.OK);

            for (int i = 0; this.checkedListBoxFun2.Items.Count > i; i++)
            {
                this.checkedListBoxFun2.SetItemChecked(i, false);
            }
            textBoxNomb.ResetText();
            comboBoxRoles.ResetText();
            this.button5.Enabled = false;
            this.button6.Enabled = false;
            this.button7.Enabled = false;
            this.checkedListBoxFun2.Enabled = false;
        }

        private void button6_Click(object sender, EventArgs e) //inhabilita el rol pasando el bit habilitado a 1
        {
                    servidor.realizarQuery("EXEC MATE_LAVADO.modificarRol_sp '" + rolSeleccionado.Nombre + "'," + 1);

                    MessageBox.Show("El Rol está habilitado.", "Rol habilitado", MessageBoxButtons.OK);

                    for (int i = 0; this.checkedListBoxFun2.Items.Count > i; i++)
                    {
                        this.checkedListBoxFun2.SetItemChecked(i, false);
                    }
                    textBoxNomb.ResetText();
                    comboBoxRoles.ResetText();
                    this.button5.Enabled = false;
                    this.button6.Enabled = false;
                    this.button7.Enabled = false;
                    this.checkedListBoxFun2.Enabled = false;
          }

        private void button7_Click(object sender, EventArgs e)
        {
            servidor.realizarQuery("EXEC MATE_LAVADO.eliminarRol_sp '" + rolSeleccionado.Nombre + "'");

            comboBoxRoles.Items.Remove(rolSeleccionado.Nombre);

            MessageBox.Show("El Rol " + rolSeleccionado.Nombre.Trim() + " ha sido eleminado.", "Rol eliminado", MessageBoxButtons.OK);

            for (int i = 0; this.checkedListBoxFun2.Items.Count > i; i++)
            {
                this.checkedListBoxFun2.SetItemChecked(i, false);
            }
            textBoxNomb.ResetText();
            comboBoxRoles.ResetText();
            this.button5.Enabled = false;
            this.button6.Enabled = false;
            this.button7.Enabled = false;
            this.checkedListBoxFun2.Enabled = false;
        }
    }
}
