using SistemaTicketsPic.Data;
using SistemaTicketsPic.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaTicketsPic.Forms
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            string correo = txtCorreo.Text.Trim();
            string clave = txtClave.Text.Trim();

            using (var db = new AppDbContext())
            {
                // Busca usuario con ese correo y contraseña

                var usuario = db.Usuarios.FirstOrDefault(u => u.Correo == correo);
                
                if (usuario == null)
                {
                    MessageBox.Show("Usuario no existe.");
                    return;
                }
                bool valido = PasswordHasher.Verify(clave, usuario.Clave);
                if (!valido)
                {
                    MessageBox.Show("Usuario y/o contraseña no existe.");
                    return;
                }
                // Guardar datos en SesionActual
                SesionActual.IdUsuario = usuario.IdUsuario;
                SesionActual.Nombre = usuario.Nombre;
                SesionActual.Correo = usuario.Correo;
                SesionActual.Cargo = usuario.Cargo;

                MessageBox.Show("¡Bienvenido " + SesionActual.Nombre + "!");

                // Abrir formulario principal y ocultar login
                FormTickets principal = new FormTickets();
                principal.Show();
                this.Hide();

            }
        }

        private void FormLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
