using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaTicketsPic.Models
{
    // Mapea la clase a la tabla Tickets en la base de datos
    [Table("Tickets")]
    public class Ticket
    {
        [Key]
        public int IdTicket { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Estado { get; set; }
        public int Prioridad { get; set; }
        public int? TecnicoId { get; set; }

        [ForeignKey("Usuario")]// Clave foránea hacia la tabla Usuario
        public int UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
