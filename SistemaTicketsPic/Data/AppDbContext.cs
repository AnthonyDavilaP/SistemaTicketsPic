using SistemaTicketsPic.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaTicketsPic.Data
{
    public class AppDbContext : DbContext
    {
        // Constructor que indica la cadena de conexión "AppDb" en el config
        public AppDbContext() : base("AppDb") { }
        // Tablas de la base de datos
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        
    } 
}
