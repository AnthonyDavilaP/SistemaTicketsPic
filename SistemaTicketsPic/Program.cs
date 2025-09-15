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
                var c = db.Database.Connection;
                if (c.State == System.Data.ConnectionState.Closed) c.Open();
                MessageBox.Show($"Servidor: {c.DataSource}\nBD: {c.Database}\nUsuarios: {db.Usuarios.Count()}");

                db.Database.Initialize(false);
                db.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);

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
            Application.Run(new Forms.FormLogin()); ;
        }
    }
}
