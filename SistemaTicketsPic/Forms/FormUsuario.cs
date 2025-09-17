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
    public partial class FormUsuario : Form
    {
        private int? _idSeleccionado = null;
        public FormUsuario()
        {
            InitializeComponent();
        }

        private void FormUsuario_Load(object sender, EventArgs e)
        {
            // Configura combobox de cargos de usuarios
            var cargos = new[]
            {
                new { Variable = "Usuario", Texto = "Usuario" },
                new { Variable = "Administrador", Texto = "Administrador" },
                new { Variable = "Tecnico", Texto = "Técnico" }
            };
            cmbCargo.DataSource = cargos;
            cmbCargo.ValueMember = "Variable";
            cmbCargo.DisplayMember = "Texto";
            // Cargar datos de usuarios en el DataGridView
            CargarDatos();
        }
        // Carga usuarios desde la base de datos al DataGridView
        private void CargarDatos()
        {
            using (var db = new AppDbContext())
            {
                dgvUsuarios.DataSource = db.Usuarios
                    .OrderBy(u => u.Nombre)
                    .Select(u => new { u.IdUsuario, u.Nombre, u.Correo, u.Cargo})
                    .ToList();
            }
            dgvUsuarios.ClearSelection();
            _idSeleccionado = null;
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }
        // Método que limpia los campos del formulario
        private void LimpiarFormulario()
        {
            txtNombre.Text = "";
            txtCorreo.Text = "";
            cmbCargo.SelectedIndex = 0;
            txtClave.Text = "";
            errorProvider1.SetError(txtNombre, "");
            errorProvider1.SetError(txtCorreo, "");
            errorProvider1.SetError(cmbCargo, "");
            _idSeleccionado = null;
        }
        // Método que valida que los campos obligatorios estén completos
        private bool ValidarFormulario()
        {
            bool ok = true;
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                errorProvider1.SetError(txtNombre, "Nombre es obligatorio");
                ok = false;
            }
            else errorProvider1.SetError(txtNombre, "");

            if (string.IsNullOrWhiteSpace(txtCorreo.Text))
            {
                errorProvider1.SetError(txtCorreo, "Correo es obligatorio");
                ok = false;
            }
            else errorProvider1.SetError(txtCorreo, "");

            if (string.IsNullOrWhiteSpace(txtClave.Text))
            {
                errorProvider1.SetError(txtClave, "Clave es obligatoria");
                ok = false;
            }
            else errorProvider1.SetError(txtClave, "");

            return ok;
        }
        // Evento al hacer clic en Limpiar, vacía el formulario
        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
            txtNombre.Focus();
        }
        // Evento al hacer clic en Guardar, crea o actualiza un usuario
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarFormulario()) return;// Validar campos obligatorios

            using (var db = new AppDbContext())
            {
                if (_idSeleccionado == null)
                {
                    var usr = new Usuario
                    {
                        Nombre = txtNombre.Text.Trim(),
                        Correo = txtCorreo.Text.Trim(),
                        Cargo = cmbCargo.SelectedValue?.ToString(),
                        Clave = PasswordHasher.Hash(txtClave.Text.Trim()) // Encriptar contraseña
                    };
                    db.Usuarios.Add(usr);
                }
                else
                {
                    // Actualiza un  usuario existente
                    var usr = db.Usuarios.Find(_idSeleccionado.Value);
                    if (usr == null) return;
                    usr.Nombre = txtNombre.Text.Trim();
                    usr.Correo = txtCorreo.Text.Trim();
                    usr.Cargo = cmbCargo.SelectedValue?.ToString();

                }
                db.SaveChanges();
            }

            CargarDatos();
            LimpiarFormulario();
        }
        // Evento al hacer clic en Editar, modifica un usuario seleccionado
        private void btnEditar_Click(object sender, EventArgs e)
        {
            using (var db = new AppDbContext())
            {
                var usr = db.Usuarios.Find(_idSeleccionado.Value);
                if (usr == null) return;
                usr.Nombre = txtNombre.Text.Trim();
                usr.Correo = txtCorreo.Text.Trim();
                usr.Cargo = cmbCargo.SelectedValue?.ToString();
                usr.Clave = PasswordHasher.Hash(txtClave.Text.Trim());
                db.SaveChanges();
            }
            CargarDatos();
            MessageBox.Show("Usuario modificado exitosamente");
            LimpiarFormulario();
        }
        // Evento al hacer clic en Eliminar: borra el usuario seleccionado
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado == null)
            {
                MessageBox.Show("Seleccione un usuario para eliminar.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var r = MessageBox.Show("¿Eliminar el usuario seleccionado?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r != DialogResult.Yes) return;

            using (var db = new AppDbContext())
            {
                var usr = db.Usuarios.Find(_idSeleccionado.Value);
                if (usr != null)
                {
                    db.Usuarios.Remove(usr);
                    db.SaveChanges();
                }
            }
            CargarDatos();
            LimpiarFormulario();
        }
        // Evento al seleccionar un usuario en el DataGridView
        private void dgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow == null || dgvUsuarios.CurrentRow.Index < 0)
                return;

            var row = dgvUsuarios.CurrentRow;
            if (row.Cells["IdUsuario"] == null) return;

            _idSeleccionado = (int?)row.Cells["IdUsuario"].Value;
            if (_idSeleccionado == null) return;

            // Mostrar datos del usuario seleccionado en los campos
            txtNombre.Text = row.Cells["Nombre"].Value?.ToString() ?? "";
            txtCorreo.Text = row.Cells["Correo"].Value?.ToString() ?? "";
            cmbCargo.Text = row.Cells["Cargo"].Value?.ToString() ?? "";
        }
    }
}
