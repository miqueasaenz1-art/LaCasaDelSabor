using System;

namespace Login_and_Register_System.Entidades
{
    public class UsuarioEntity
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Contrasena { get; set; }
        public string Rol { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; }
    }
}

