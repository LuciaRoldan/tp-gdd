﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PalcoNet.Abm_Cliente
{
    public partial class ModificarCli : Form
    {
        public ModificarCli()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new BusquedaCli().Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Aca hay que hacer que realmente cambien las cosas

            new BusquedaCli().Show();
            this.Hide();
        }
    }
}
