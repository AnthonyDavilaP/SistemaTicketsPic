using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaTicketsPic.Models
{
    // Mapea la clase a la tabla Usuarios en la base de datos
    [Table("Usuarios")]
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Cargo { get; set; }
        public string Clave { get; set; }
        // Relación uno a muchos, un usuario puede tener varios tickets
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
