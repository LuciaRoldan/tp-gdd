﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PalcoNet.Abm_Empresa_Espectaculo
{
    public partial class ResultadosBusquedaEmp : Form
    {
        public ResultadosBusquedaEmp()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new SeleccionarFuncionalidad().Show();
            this.Hide(); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new BusquedaEmp().Show();
            this.Hide(); 
        }
    }
}
