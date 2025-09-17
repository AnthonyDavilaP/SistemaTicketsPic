using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaTicketsPic.Utils
{
    internal class SesionActual
    {
        // Propiedades estáticas para almacenar los datos del usuario logueado
        public static int IdUsuario { get; set; }
        public static string Nombre { get; set; } = string.Empty;
        public static string Correo { get; set; } = string.Empty;
        public static string Cargo { get; set; } = string.Empty;

        //Indica si hay un usuario logueado en la aplicación.
        public static bool EstaLogueado => IdUsuario > 0;

        //Indica si el usuario actual es administrador.
        public static bool EsAdministrador => Cargo.ToUpper() == "ADMINISTRADOR";
        public static bool EsTecnico => Cargo.ToUpper() == "TECNICO";
        public static bool EsUsuario => Cargo.ToUpper() == "USUARIO";
        // Cierra la sesión actual.
        public static void CerrarSesion()
        {
            IdUsuario = 0;
            Nombre = string.Empty;
            Correo = string.Empty;
            Cargo = string.Empty;
        }
    }
}
