﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using PalcoNet.Dominio;

namespace PalcoNet.Registro_de_Usuario
{
    public partial class RegistroDeEmpresa : MiForm
    {
        Empresa empresa;

        internal Empresa Empresa
        {
            get { return empresa; }
            set { empresa = value; }
        } 

        public RegistroDeEmpresa(Empresa empresa, MiForm formAnterior) : base(formAnterior)
        {
            this.empresa = empresa;
            InitializeComponent();
        }

        private RegistroDeEmpresa() { }

        private bool camposCompletos()
        {
            string error = "";
            long x;
            if (string.IsNullOrWhiteSpace(textBoxRazonSocial.Text)) { error += "La Razón Social no puede estar vacía.\n"; }
            if (string.IsNullOrWhiteSpace(textBoxCUIT.Text)) { error += "El CUIT no puede estar vacío.\n"; }
            if (!long.TryParse(textBoxCUIT.Text, out x)) { error += "El CUIT no puede estar vacío.\n"; }

            if (error != "")
            {
                MessageBox.Show(error, "Error", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new SeleccionarFuncionalidad().Show();
            this.Hide();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
            if (this.Anterior == null) { new LogIn().Show(); } else { this.Anterior.Show(); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.camposCompletos()){
                Empresa.RazonSocial = textBoxRazonSocial.Text;
                Empresa.Cuit = Int64.Parse(textBoxCUIT.Text);
                Empresa.FechaDeCreacion = DateTime.Now;
                if (!string.IsNullOrWhiteSpace(textBoxMail.Text)) { Empresa.Mail = textBoxMail.Text; }

                
                if (string.IsNullOrWhiteSpace(empresa.NombreUsuario))
                {
                    empresa.NombreUsuario = textBoxCUIT.Text;
                   
                    StringBuilder Sb = new StringBuilder();
                    using (SHA256 hash = SHA256Managed.Create())
                    {
                        Encoding enc = Encoding.UTF8;
                        Byte[] result = hash.ComputeHash(enc.GetBytes(textBoxCUIT.Text.ToString()));

                        foreach (Byte b in result)
                            Sb.Append(b.ToString("x2"));

                        Console.WriteLine("EL HASH ES:" + Sb.ToString());
                    }
                    empresa.Contrasenia = Sb.ToString();
                    empresa.DebeCambiarContraseña = true;
                }

                new RegistroDomicilio(this, this.Empresa).Show();
                this.Hide();
            }
        }

    }
}