using SistemaTicketsPic.Data;
using SistemaTicketsPic.Models;
using SistemaTicketsPic.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaTicketsPic.Forms 
{
    public partial class FormTickets : Form
    {
        private AppDbContext _context;
        private int? _idSeleccionado = null;

        public FormTickets()
        {
            InitializeComponent();
            _context = new AppDbContext();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            usuariosToolStripMenuItem.Visible = SesionActual.EsAdministrador;
            if(SesionActual.EsTecnico || SesionActual.EsAdministrador)
            {
                btnActualizar.Enabled = true;
            }
            else
            {
                btnActualizar.Enabled = false;
            }
            if (SesionActual.EsUsuario || SesionActual.EsAdministrador)
            {
                btnNuevoTicket.Enabled = true;
            }
            else
            {
                btnNuevoTicket.Enabled = false;
            }
            if (SesionActual.EsTecnico)
            {
                txtDescripcion.Enabled = false;
                txtPrioridad.Enabled = false;
            }
            else
            {
                txtDescripcion.Enabled = true;
                txtPrioridad.Enabled = true;
            }
            if (SesionActual.EsUsuario)
            {
                cmbEstado.Enabled = false;
            }
            else
            {
                cmbEstado.Enabled = true;
            }
            var estados = new[]
            {
                new { Variable = "Abierto", Texto = "Abierto" },
                new { Variable = "Cerrado", Texto = "Cerrado" }
            };
            cmbEstado.DataSource = estados;
            cmbEstado.ValueMember = "Variable";
            cmbEstado.DisplayMember = "Texto";
            CargarTickets();
        }
        private void CargarTickets()
        {
            using (var db = new AppDbContext())
            {
                dgvTickets.DataSource = db.Tickets
                    .OrderBy(u => u.FechaCreacion)
                    .Select(u => new { u.IdTicket, u.Descripcion, u.Prioridad, u.FechaCreacion, u.Estado, u.Usuario.Nombre})
                    .ToList();
            }
            dgvTickets.ClearSelection();
            _idSeleccionado = null;
        }
        private void LimpiarFormulario()
        {
            txtDescripcion.Text = "";
            txtPrioridad.Text = "";
            cmbEstado.SelectedIndex = 0;
            _idSeleccionado = null;
        }
        private void btnNuevoTicket_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDescripcion.Text) || string.IsNullOrWhiteSpace(txtPrioridad.Text))
            {
                MessageBox.Show("Complete descripción y prioridad.");
                return;
            }

            if (!int.TryParse(txtPrioridad.Text, out int prio))
            {
                MessageBox.Show("Prioridad debe ser un número entero.");
                return;
            }

            using (var db = new AppDbContext())
            {
                var ticket = new Ticket

                {
                    Descripcion = txtDescripcion.Text,
                    FechaCreacion = DateTime.Now,
                    Estado = "Abierto",
                    Prioridad = prio,
                    UsuarioId = SesionActual.IdUsuario
                };
                db.Tickets.Add(ticket);
                db.SaveChanges();
            }           
            CargarTickets();
            MessageBox.Show("Ticket creado exitosamente");
            LimpiarFormulario();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _context.Dispose();
        }

        private void cerrarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Desea cerrar la sesión?", "Confirmar cierre de sesión",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                SesionActual.CerrarSesion();
                this.Hide();
                var login = new FormLogin();
                login.Show();
            }
        }
        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new FormUsuario();
           
            frm.Show();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();   
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTickets.CurrentRow == null || dgvTickets.CurrentRow.Index < 0)
                return;

            var row = dgvTickets.CurrentRow;
            if (row.Cells["IdTicket"] == null) return;

            _idSeleccionado = (int?)row.Cells["IdTicket"].Value;
            if (_idSeleccionado == null) return;


            txtDescripcion.Text = row.Cells["Descripcion"].Value?.ToString() ?? "";
            txtPrioridad.Text = row.Cells["Prioridad"].Value?.ToString() ?? "";
            cmbEstado.Text = row.Cells["Estado"].Value?.ToString() ?? "";
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            using (var db = new AppDbContext())
            {
                var ticket = db.Tickets.Find(_idSeleccionado.Value);
                if (ticket == null) return;               
                ticket.Estado = cmbEstado.SelectedValue?.ToString();   
                db.SaveChanges();
            }
            CargarTickets();
            MessageBox.Show("Ticket modificado exitosamente");
            LimpiarFormulario();
        }

        private void FormTickets_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit(); 
        }
    }
}
