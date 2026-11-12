using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using Microsoft.Win32;

namespace Login_and_Register_System.DAL
{
    /// <summary>
    /// Centraliza la conexión a SQL Server (local o en la PC de un compañero en red),
    /// detecta instancias locales automáticamente cuando hace falta y adapta consultas
    /// al esquema de LaCasaDelSabor.
    /// </summary>
    public static class ConexionBD
    {
        private const string NombreCadenaConfig = "LaCasaDelSabor";
        private const string CadenaPorDefecto =
            @"Server=localhost;Database=LaCasaDelSabor;Integrated Security=True;Connect Timeout=10;TrustServerCertificate=True;";

        private static readonly ConcurrentDictionary<string, bool> _cacheTieneStock =
            new ConcurrentDictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

        private static readonly object _lockLocal = new object();
        private static string _claveConfigResuelta;
        private static string _servidorLocalResuelto;

        private static string RutaConfigLocal
        {
            get
            {
                string carpeta = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "LaCasaDelSabor");
                return Path.Combine(carpeta, "conexion.xml");
            }
        }

        private static string LeerCadenaDesdeAppConfig()
        {
            var settings = ConfigurationManager.ConnectionStrings[NombreCadenaConfig];
            if (settings != null && !string.IsNullOrWhiteSpace(settings.ConnectionString))
            {
                return settings.ConnectionString;
            }
            return CadenaPorDefecto;
        }

        private static string ConstruirCadena(
            string servidor,
            string baseDatos,
            bool autenticacionWindows,
            string usuario,
            string contrasena,
            int timeoutSegundos)
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = (servidor ?? "").Trim(),
                InitialCatalog = string.IsNullOrWhiteSpace(baseDatos) ? "LaCasaDelSabor" : baseDatos.Trim(),
                IntegratedSecurity = autenticacionWindows,
                ConnectTimeout = timeoutSegundos,
                TrustServerCertificate = true
            };

            if (!autenticacionWindows)
            {
                builder.UserID = (usuario ?? "").Trim();
                builder.Password = contrasena ?? "";
            }

            return builder.ConnectionString;
        }

        private static string ObtenerCadenaBaseConfigurada()
        {
            string appConfigActual = LeerCadenaDesdeAppConfig();

            try
            {
                if (File.Exists(RutaConfigLocal))
                {
                    var doc = XDocument.Load(RutaConfigLocal);
                    var raiz = doc.Root;
                    if (raiz != null)
                    {
                        string appConfigBase = (string)raiz.Element("AppConfigBase") ?? "";
                        if (string.Equals(appConfigBase, appConfigActual, StringComparison.Ordinal))
                        {
                            string servidor = (string)raiz.Element("Servidor") ?? "";
                            if (!string.IsNullOrWhiteSpace(servidor))
                            {
                                string baseDatos = (string)raiz.Element("BaseDatos") ?? "LaCasaDelSabor";
                                bool authWin = string.Equals(
                                    (string)raiz.Element("AutenticacionWindows"),
                                    "true",
                                    StringComparison.OrdinalIgnoreCase);
                                string usuario = (string)raiz.Element("Usuario") ?? "";
                                string cifrada = (string)raiz.Element("Contrasena") ?? "";
                                string contrasena = Descifrar(cifrada);
                                return ConstruirCadena(servidor, baseDatos, authWin, usuario, contrasena, 10);
                            }
                        }
                    }
                }
            }
            catch
            {
                // Si el archivo local está dañado, se usa App.config sin bloquear el arranque.
            }

            return appConfigActual;
        }

        public static bool TieneOverrideLocal()
        {
            string appConfigActual = LeerCadenaDesdeAppConfig();
            try
            {
                if (!File.Exists(RutaConfigLocal)) return false;
                var doc = XDocument.Load(RutaConfigLocal);
                var raiz = doc.Root;
                if (raiz == null) return false;
                string appConfigBase = (string)raiz.Element("AppConfigBase") ?? "";
                string servidor = (string)raiz.Element("Servidor") ?? "";
                return string.Equals(appConfigBase, appConfigActual, StringComparison.Ordinal)
                    && !string.IsNullOrWhiteSpace(servidor);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Devuelve la cadena de conexión activa. Si apunta a un servidor local (ej. localhost)
        /// pero la base de datos está en .\SQLEXPRESS, LocalDB u otra instancia local instalada,
        /// detecta y usa automáticamente esa instancia.
        /// </summary>
        public static string ObtenerCadenaConexion()
        {
            string cadenaBase = ObtenerCadenaBaseConfigurada();
            return ResolverCadenaLocalSiEsNecesario(cadenaBase);
        }

        public static SqlConnection CrearConexion()
        {
            return new SqlConnection(ObtenerCadenaConexion());
        }

        public static SqlConnectionStringBuilder ObtenerConfiguracionActual()
        {
            return ObtenerConfiguracionSinSondeo();
        }

        /// <summary>
        /// Lee la configuración actual sin bloquear la UI probando conexiones de red.
        /// Si ya se detectó una instancia local activa en esta sesión, la refleja.
        /// </summary>
        public static SqlConnectionStringBuilder ObtenerConfiguracionSinSondeo()
        {
            try
            {
                string cadenaBase = ObtenerCadenaBaseConfigurada();
                var builder = new SqlConnectionStringBuilder(cadenaBase);
                lock (_lockLocal)
                {
                    if (string.Equals(_claveConfigResuelta, cadenaBase, StringComparison.Ordinal) &&
                        !string.IsNullOrWhiteSpace(_servidorLocalResuelto))
                    {
                        builder.DataSource = _servidorLocalResuelto;
                    }
                }
                return builder;
            }
            catch
            {
                return new SqlConnectionStringBuilder(CadenaPorDefecto);
            }
        }

        /// <summary>
        /// Fuerza la detección del servidor local (útil en segundo plano al abrir LoginForm).
        /// </summary>
        public static void AsegurarServidorLocal()
        {
            ObtenerCadenaConexion();
        }

        public static string ObtenerResumenServidor()
        {
            var cfg = ObtenerConfiguracionActual();
            string servidor = string.IsNullOrWhiteSpace(cfg.DataSource) ? "localhost" : cfg.DataSource;
            string db = string.IsNullOrWhiteSpace(cfg.InitialCatalog) ? "LaCasaDelSabor" : cfg.InitialCatalog;
            return servidor + " • " + db;
        }

        public static string ObtenerDescripcionConexion()
        {
            var cfg = ObtenerConfiguracionActual();
            string servidor = string.IsNullOrWhiteSpace(cfg.DataSource) ? "localhost" : cfg.DataSource;
            string db = string.IsNullOrWhiteSpace(cfg.InitialCatalog) ? "LaCasaDelSabor" : cfg.InitialCatalog;
            string modo = cfg.IntegratedSecurity ? "Windows" : ("SQL: " + cfg.UserID);
            return string.Format("{0} (Servidor: {1} • {2})", db, servidor, modo);
        }

        public static List<string> ObtenerInstanciasLocalesCandidatas()
        {
            var lista = new List<string>();
            var vistos = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            Action<string> agregar = s =>
            {
                if (!string.IsNullOrWhiteSpace(s) && vistos.Add(s.Trim()))
                    lista.Add(s.Trim());
            };

            LeerInstanciasDesdeRegistro(RegistryView.Registry64, agregar);
            LeerInstanciasDesdeRegistro(RegistryView.Registry32, agregar);

            agregar("localhost");
            agregar(@".\SQLEXPRESS");
            agregar(@"localhost\SQLEXPRESS");
            agregar(@"(localdb)\MSSQLLocalDB");
            agregar(@"(localdb)\ProjectsV13");
            agregar(@".\MSSQLSERVER01");

            return lista;
        }

        private static void LeerInstanciasDesdeRegistro(RegistryView vista, Action<string> agregar)
        {
            try
            {
                using (var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, vista))
                using (var sqlKey = baseKey.OpenSubKey(@"SOFTWARE\Microsoft\Microsoft SQL Server\Instance Names\SQL"))
                {
                    if (sqlKey == null) return;
                    foreach (string nombre in sqlKey.GetValueNames())
                    {
                        if (string.Equals(nombre, "MSSQLSERVER", StringComparison.OrdinalIgnoreCase))
                        {
                            agregar("localhost");
                            agregar(".");
                        }
                        else if (!string.IsNullOrWhiteSpace(nombre))
                        {
                            agregar(@".\" + nombre);
                            agregar(@"localhost\" + nombre);
                        }
                    }
                }
            }
            catch
            {
                // Sin acceso al registro se continúa con los nombres estándar.
            }
        }

        private static bool EsServidorLocal(string dataSource)
        {
            if (string.IsNullOrWhiteSpace(dataSource)) return true;

            string s = dataSource.Trim();
            string host = s.Split('\\', ',')[0].Trim();

            return string.Equals(host, "localhost", StringComparison.OrdinalIgnoreCase)
                || string.Equals(host, ".", StringComparison.Ordinal)
                || string.Equals(host, "(local)", StringComparison.OrdinalIgnoreCase)
                || string.Equals(host, "127.0.0.1", StringComparison.Ordinal)
                || string.Equals(host, "(localdb)", StringComparison.OrdinalIgnoreCase)
                || string.Equals(host, Environment.MachineName, StringComparison.OrdinalIgnoreCase);
        }

        public static string DetectarInstanciaLocal(string baseDatos, out bool encontroBaseDatos)
        {
            string bd = string.IsNullOrWhiteSpace(baseDatos) ? "LaCasaDelSabor" : baseDatos.Trim();
            var plantilla = new SqlConnectionStringBuilder
            {
                InitialCatalog = bd,
                IntegratedSecurity = true,
                ConnectTimeout = 2,
                TrustServerCertificate = true
            };
            return BuscarMejorInstanciaLocal(ObtenerInstanciasLocalesCandidatas(), bd, plantilla, out encontroBaseDatos);
        }

        private static string BuscarMejorInstanciaLocal(
            IEnumerable<string> candidatos,
            string bdObjetivo,
            SqlConnectionStringBuilder plantilla,
            out bool encontroBaseDatos)
        {
            encontroBaseDatos = false;
            string primerServidorVivo = null;

            foreach (string candidato in candidatos)
            {
                bool tieneBd;
                if (ProbarInstancia(candidato, bdObjetivo, plantilla, out tieneBd))
                {
                    if (tieneBd)
                    {
                        encontroBaseDatos = true;
                        return candidato;
                    }
                    if (primerServidorVivo == null)
                    {
                        primerServidorVivo = candidato;
                    }
                }
            }

            return primerServidorVivo;
        }

        private static string ResolverCadenaLocalSiEsNecesario(string cadenaBase)
        {
            SqlConnectionStringBuilder cfg;
            try
            {
                cfg = new SqlConnectionStringBuilder(cadenaBase);
            }
            catch
            {
                return cadenaBase;
            }

            if (!EsServidorLocal(cfg.DataSource))
            {
                return cadenaBase;
            }

            lock (_lockLocal)
            {
                if (string.Equals(_claveConfigResuelta, cadenaBase, StringComparison.Ordinal) &&
                    !string.IsNullOrWhiteSpace(_servidorLocalResuelto))
                {
                    cfg.DataSource = _servidorLocalResuelto;
                    return cfg.ConnectionString;
                }

                string bdObjetivo = string.IsNullOrWhiteSpace(cfg.InitialCatalog)
                    ? "LaCasaDelSabor"
                    : cfg.InitialCatalog;

                var candidatos = new List<string>();
                if (!string.IsNullOrWhiteSpace(cfg.DataSource))
                {
                    candidatos.Add(cfg.DataSource);
                }
                foreach (string inst in ObtenerInstanciasLocalesCandidatas())
                {
                    if (!candidatos.Exists(c => string.Equals(c, inst, StringComparison.OrdinalIgnoreCase)))
                    {
                        candidatos.Add(inst);
                    }
                }

                bool encontroBd;
                string elegido = BuscarMejorInstanciaLocal(candidatos, bdObjetivo, cfg, out encontroBd);
                if (!string.IsNullOrWhiteSpace(elegido))
                {
                    _claveConfigResuelta = cadenaBase;
                    _servidorLocalResuelto = elegido;
                    cfg.DataSource = elegido;
                    return cfg.ConnectionString;
                }
            }

            return cadenaBase;
        }

        private static bool ProbarInstancia(
            string servidor,
            string baseDatosObjetivo,
            SqlConnectionStringBuilder plantilla,
            out bool existeBaseDatos)
        {
            existeBaseDatos = false;
            try
            {
                var prueba = new SqlConnectionStringBuilder(plantilla.ConnectionString)
                {
                    DataSource = servidor,
                    InitialCatalog = baseDatosObjetivo,
                    ConnectTimeout = 2,
                    TrustServerCertificate = true
                };

                using (var cn = new SqlConnection(prueba.ConnectionString))
                {
                    cn.Open();
                    existeBaseDatos = true;
                    return true;
                }
            }
            catch (SqlException ex)
            {
                // Error 4060: el servidor respondió, pero esa base de datos no está en esta instancia.
                if (ex.Number == 4060)
                {
                    existeBaseDatos = false;
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static void ProbarConexion(
            string servidor,
            string baseDatos,
            bool autenticacionWindows,
            string usuario,
            string contrasena)
        {
            if (string.IsNullOrWhiteSpace(servidor))
                throw new ArgumentException("Ingresa la IP, nombre de PC o servidor SQL.");
            if (string.IsNullOrWhiteSpace(baseDatos))
                throw new ArgumentException("Ingresa el nombre de la base de datos.");
            if (!autenticacionWindows && string.IsNullOrWhiteSpace(usuario))
                throw new ArgumentException("Ingresa el usuario de SQL Server.");

            string cadenaPrueba = ConstruirCadena(
                servidor, baseDatos, autenticacionWindows, usuario, contrasena, 5);

            using (var cn = new SqlConnection(cadenaPrueba))
            {
                cn.Open();
                using (var cmd = new SqlCommand("SELECT 1", cn))
                {
                    cmd.ExecuteScalar();
                }
            }
        }

        public static void GuardarConfiguracion(
            string servidor,
            string baseDatos,
            bool autenticacionWindows,
            string usuario,
            string contrasena)
        {
            if (string.IsNullOrWhiteSpace(servidor))
                throw new ArgumentException("Ingresa la IP, nombre de PC o servidor SQL.");
            if (string.IsNullOrWhiteSpace(baseDatos))
                throw new ArgumentException("Ingresa el nombre de la base de datos.");
            if (!autenticacionWindows && string.IsNullOrWhiteSpace(usuario))
                throw new ArgumentException("Ingresa el usuario de SQL Server.");

            string carpeta = Path.GetDirectoryName(RutaConfigLocal);
            if (!string.IsNullOrEmpty(carpeta) && !Directory.Exists(carpeta))
            {
                Directory.CreateDirectory(carpeta);
            }

            var doc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("ConexionBD",
                    new XElement("AppConfigBase", LeerCadenaDesdeAppConfig()),
                    new XElement("Servidor", servidor.Trim()),
                    new XElement("BaseDatos", baseDatos.Trim()),
                    new XElement("AutenticacionWindows", autenticacionWindows ? "true" : "false"),
                    new XElement("Usuario", autenticacionWindows ? "" : (usuario ?? "").Trim()),
                    new XElement("Contrasena", autenticacionWindows ? "" : Cifrar(contrasena ?? ""))
                ));

            doc.Save(RutaConfigLocal);
            LimpiarCache();
        }

        public static void RestablecerConfiguracion()
        {
            if (File.Exists(RutaConfigLocal))
            {
                File.Delete(RutaConfigLocal);
            }
            LimpiarCache();
        }

        public static void LimpiarCache()
        {
            lock (_lockLocal)
            {
                _claveConfigResuelta = null;
                _servidorLocalResuelto = null;
            }
            _cacheTieneStock.Clear();
        }

        /// <summary>
        /// Verifica si la tabla Productos cuenta con la columna 'stock' en el servidor conectado.
        /// El script base de LaCasaDelSabor no incluye 'stock', por lo que permite compatibilidad con ambos esquemas.
        /// </summary>
        public static bool TieneColumnaStock(SqlConnection cn, SqlTransaction tx = null)
        {
            if (cn == null) throw new ArgumentNullException("cn");

            string clave = cn.ConnectionString ?? "";
            bool valorCache;
            if (_cacheTieneStock.TryGetValue(clave, out valorCache))
            {
                return valorCache;
            }

            using (var cmd = new SqlCommand(
                "SELECT CASE WHEN COL_LENGTH('dbo.Productos', 'stock') IS NOT NULL THEN 1 ELSE 0 END",
                cn,
                tx))
            {
                bool tiene = Convert.ToInt32(cmd.ExecuteScalar()) == 1;
                _cacheTieneStock[clave] = tiene;
                return tiene;
            }
        }

        private static string Cifrar(string textoPlano)
        {
            if (string.IsNullOrEmpty(textoPlano)) return "";
            byte[] datos = Encoding.UTF8.GetBytes(textoPlano);
            byte[] protegidos = ProtectedData.Protect(datos, null, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(protegidos);
        }

        private static string Descifrar(string base64)
        {
            if (string.IsNullOrEmpty(base64)) return "";
            try
            {
                byte[] protegidos = Convert.FromBase64String(base64);
                byte[] datos = ProtectedData.Unprotect(protegidos, null, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(datos);
            }
            catch
            {
                return "";
            }
        }
    }
}
