using SistemaTicketsPic.Data;
using SistemaTicketsPic.Forms;
using SistemaTicketsPic.Models;
using SistemaTicketsPic.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaTicketsPic
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var db = new AppDbContext())
            {
                // Obtiene la conexión a la base de datos
                var c = db.Database.Connection;
                if (c.State == System.Data.ConnectionState.Closed) c.Open();
                MessageBox.Show($"Servidor: {c.DataSource}\nBD: {c.Database}\nUsuarios: {db.Usuarios.Count()}");

                db.Database.Initialize(false);
                db.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
                // Crea un usuario administrador por defecto si no hay usuarios
                if (!db.Usuarios.Any())
                {
                    db.Usuarios.Add(new Usuario
                    {
                        Correo = "admin@iti.edu.ec",
                        Clave = PasswordHasher.Hash("1234"),
                        Cargo = "Administrador"
                    });
                    db.SaveChanges();
                }
            }
            // Inicia la aplicación mostrando el formulario de login
            Application.Run(new Forms.FormLogin()); ;
        }
    }
}
