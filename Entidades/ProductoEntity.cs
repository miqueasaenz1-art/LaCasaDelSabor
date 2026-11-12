namespace Login_and_Register_System.Entidades
{
    public class ProductoEntity
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public bool Disponibilidad { get; set; } = true;
        public int IdCategoria { get; set; }
        public string NombreCategoria { get; set; }
        public int? Stock { get; set; }
    }

    public class CategoriaEntity
    {
        public int IdCategoria { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
    }
}

