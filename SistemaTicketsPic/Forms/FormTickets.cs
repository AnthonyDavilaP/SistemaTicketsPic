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
            // Configura visibilidad y habilitación de controles según el rol
            usuariosToolStripMenuItem.Visible = SesionActual.EsAdministrador;
            usuariosToolStripMenuItem.Visible = SesionActual.EsAdministrador;
            btnActualizar.Enabled = SesionActual.EsTecnico || SesionActual.EsAdministrador;
            btnCrear.Enabled = SesionActual.EsUsuario || SesionActual.EsAdministrador;
            txtDescripcion.Enabled = txtPrioridad.Enabled = !SesionActual.EsTecnico;
            btnEliminar.Enabled = SesionActual.EsAdministrador;
            cmbEstado.Enabled = !SesionActual.EsUsuario;

            // Opciones del combobox Estado del Ticket
            cmbEstado.DataSource = new[] { "Abierto", "Cerrado" };

            // Cargar técnicos para asignar 
            using (var db = new AppDbContext())
            {
                var tecnicos = db.Usuarios
                                 .Where(u => u.Cargo == "Tecnico")
                                 .Select(u => new { u.IdUsuario, u.Nombre })
                                 .ToList();

                cmbTecnico.DataSource = tecnicos;
                cmbTecnico.ValueMember = "IdUsuario";
                cmbTecnico.DisplayMember = "Nombre";
            }
            // Solo Admin puede usarlo, los demás lo ven deshabilitado
            cmbTecnico.Enabled = SesionActual.EsAdministrador;
            // Cargar tickets en el DataGridView
            CargarTickets();
        }
        // Método para cargar todos los tickets en el DataGridView
        private void CargarTickets()
        {
            using (var db = new AppDbContext())
            {
                dgvTickets.DataSource = db.Tickets
                    .OrderBy(u => u.FechaCreacion)
                    .Select(u => new { u.IdTicket, u.Descripcion, u.Prioridad, u.FechaCreacion, u.Estado, u.TecnicoId,u.Usuario.Nombre})
                    .ToList();
            }
            dgvTickets.ClearSelection();
            _idSeleccionado = null;
            dgvTickets.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
        // Método para limpiar los campos del formulario
        private void LimpiarFormulario()
        {
            txtDescripcion.Text = "";
            txtPrioridad.Text = "";
            cmbEstado.SelectedIndex = 0;
            _idSeleccionado = null;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _context.Dispose(); // Liberar recursos al cerrar, y cerrar correctamente.
        }
        // Cerrar sesión y volver al login
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
            Application.Exit(); // Salir de la aplicación
        }
        // Abrir formulario de gestión de usuarios
        private void usuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new FormUsuario();
           
            frm.Show();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();   // Limpiar campos con el boton 
        }
        // Actualiza campos del DataGrid al seleccionar un ticket
        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            // Verifica que haya una fila seleccionada válida
            if (dgvTickets.CurrentRow == null || dgvTickets.CurrentRow.Index < 0)
                return;

            var row = dgvTickets.CurrentRow;
            if (row.Cells["IdTicket"] == null) return;

            _idSeleccionado = (int?)row.Cells["IdTicket"].Value;
            if (_idSeleccionado == null) return;

            // Rellena los campos del formulario con la información del ticket
            txtDescripcion.Text = row.Cells["Descripcion"].Value?.ToString() ?? "";
            txtPrioridad.Text = row.Cells["Prioridad"].Value?.ToString() ?? "";
            cmbEstado.Text = row.Cells["Estado"].Value?.ToString() ?? "";
        }
        // Actualiza un ticket existente con el boton
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            using (var db = new AppDbContext())
            {
                // Solo el usuario tipo Administrador y técnico pueden cambiar el estado
                var ticket = db.Tickets.Find(_idSeleccionado.Value);
                if (ticket == null) return;               
                ticket.Estado = cmbEstado.SelectedValue?.ToString();
                // Solo el usuario tipo Administrador puede asignar un técnico
                if (SesionActual.EsAdministrador && cmbTecnico.SelectedValue != null)
                {
                    ticket.TecnicoId = (int)cmbTecnico.SelectedValue;
                }
                db.SaveChanges();
            }
            CargarTickets();
            MessageBox.Show("Ticket modificado exitosamente");
            LimpiarFormulario();
        }

        private void FormTickets_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit(); // Cerrar aplicación al cerrar el formulario
        }
        // Elimina el ticket seleccionado solo el administrador 
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado == null)
            {
                MessageBox.Show("Seleccione un ticket para eliminar.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var r = MessageBox.Show("¿Eliminar el ticket seleccionado?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r != DialogResult.Yes) return;

            using (var db = new AppDbContext())
            {
                var tck = db.Tickets.Find(_idSeleccionado.Value);
                if (tck != null)
                {
                    db.Tickets.Remove(tck);
                    db.SaveChanges();
                }
            }
            CargarTickets();
            LimpiarFormulario();
        }
        // Crear un nuevo ticket mendiante boton
        private void btnCrear_Click_1(object sender, EventArgs e)
        {
            // Se valida campos obligatorios
            if (string.IsNullOrWhiteSpace(txtDescripcion.Text) || string.IsNullOrWhiteSpace(txtPrioridad.Text))
            {
                MessageBox.Show("Complete descripción y prioridad.");
                return;
            }
            // Se valida que la prioridad sea un número entero
            if (!int.TryParse(txtPrioridad.Text, out int prioridad))
            {
                MessageBox.Show("Prioridad debe ser un número entero.");
                return;
            }
            // Crear y guardar el ticket en la base de datos
            using (var db = new AppDbContext())
            {
                var ticket = new Ticket

                {
                    Descripcion = txtDescripcion.Text,
                    FechaCreacion = DateTime.Now,
                    Estado = "Abierto",
                    Prioridad = prioridad,
                    UsuarioId = SesionActual.IdUsuario
                };
                db.Tickets.Add(ticket);
                db.SaveChanges();
            }
            CargarTickets();
            MessageBox.Show("Ticket creado exitosamente");
            LimpiarFormulario();
        }
    }
}
