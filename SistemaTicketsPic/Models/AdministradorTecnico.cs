using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaTicketsPic.Models
{
    public class AdministradorTecnico : Usuario
    {
        public string AsignarTecnico(Ticket t, int tecnicoId)
        {
            t.TecnicoId = tecnicoId;
            t.Estado = "En Proceso";
            return "Técnico asignado correctamente";
        }
    }

    public class Tecnico : Usuario
    {
        public string ResolverTicket(Ticket t)
        {
            t.Estado = "Cerrado";
            return "Ticket resuelto";
        }
    }
}

