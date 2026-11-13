using System;
using System.Collections.Generic;
using System.Net.Mail;
using Login_and_Register_System.DAL;
using Login_and_Register_System.Entidades;

namespace Login_and_Register_System.BLL
{
    public class UsuarioBLL
    {
        public static readonly string[] RolesDisponibles = { "Administrador", "Cajero", "Pastelería" };

        public static bool EsAdmin(string rol)
        {
            if (string.IsNullOrWhiteSpace(rol)) return false;
            string r = rol.Trim().ToLowerInvariant();
            return r.Contains("admin");
        }

        public static bool EsCajero(string rol)
        {
            if (string.IsNullOrWhiteSpace(rol)) return false;
            string r = rol.Trim().ToLowerInvariant();
            return r.Contains("cajer");
        }

        public static bool EsPasteleria(string rol)
        {
            if (string.IsNullOrWhiteSpace(rol)) return false;
            string r = rol.Trim().ToLowerInvariant();
            return r.Contains("pastel") || r.Contains("cocin") || r.Contains("repost");
        }

        private readonly UsuarioDAL dal = new UsuarioDAL();

        public List<UsuarioEntity> ObtenerTodos()
        {
            return dal.ObtenerTodos();
        }

        public UsuarioEntity ObtenerPorId(int idUsuario)
        {
            return dal.ObtenerPorId(idUsuario);
        }

        public int Guardar(UsuarioEntity usuario)
        {
            if (usuario == null)
                throw new ArgumentNullException("usuario");
            if (string.IsNullOrWhiteSpace(usuario.Nombre))
                throw new ArgumentException("Ingresa el nombre completo del usuario.");
            if (string.IsNullOrWhiteSpace(usuario.Correo))
                throw new ArgumentException("Ingresa el correo electrónico.");

            string correoLimpio = usuario.Correo.Trim();
            if (!EsCorreoValido(correoLimpio))
                throw new ArgumentException("Ingresa un correo electrónico válido.");

            if (string.IsNullOrWhiteSpace(usuario.Rol))
                throw new ArgumentException("Selecciona un rol.");

            bool esNuevo = usuario.IdUsuario <= 0;
            bool actualizarPass = !string.IsNullOrWhiteSpace(usuario.Contrasena);

            if (esNuevo && !actualizarPass)
                throw new ArgumentException("Ingresa una contraseña para el nuevo usuario.");

            if (actualizarPass && usuario.Contrasena.Trim().Length < 4)
                throw new ArgumentException("La contraseña debe tener al menos 4 caracteres.");

            if (dal.ExisteCorreo(correoLimpio, esNuevo ? (int?)null : usuario.IdUsuario))
                throw new ArgumentException("Ya existe otro usuario registrado con ese correo.");

            if (!esNuevo)
            {
                bool sigueSiendoAdminActivo =
                    usuario.Activo &&
                    string.Equals(usuario.Rol.Trim(), "Administrador", StringComparison.OrdinalIgnoreCase);

                if (!sigueSiendoAdminActivo && dal.ContarAdministradoresActivos(usuario.IdUsuario) == 0)
                {
                    throw new ArgumentException("Debe permanecer al menos un Administrador activo en el sistema.");
                }

                dal.Actualizar(usuario, actualizarPass);
                return usuario.IdUsuario;
            }

            return dal.Insertar(usuario);
        }

        public void Eliminar(int idUsuario, string nombreSesionActual)
        {
            var objetivo = dal.ObtenerPorId(idUsuario);
            if (objetivo == null)
                throw new ArgumentException("El usuario ya no existe.");

            if (!string.IsNullOrWhiteSpace(nombreSesionActual) &&
                string.Equals(objetivo.Nombre, nombreSesionActual, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException("No puedes eliminar el usuario con el que iniciaste sesión.");
            }

            if (objetivo.Activo &&
                string.Equals(objetivo.Rol, "Administrador", StringComparison.OrdinalIgnoreCase) &&
                dal.ContarAdministradoresActivos(idUsuario) == 0)
            {
                throw new ArgumentException("No puedes eliminar el único Administrador activo.");
            }

            dal.Eliminar(idUsuario);
        }

        private static bool EsCorreoValido(string correo)
        {
            try
            {
                var addr = new MailAddress(correo);
                return string.Equals(addr.Address, correo, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }
    }
}

