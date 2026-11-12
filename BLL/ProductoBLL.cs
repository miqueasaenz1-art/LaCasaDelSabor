using System;
using System.Collections.Generic;
using Login_and_Register_System.DAL;
using Login_and_Register_System.Entidades;

namespace Login_and_Register_System.BLL
{
    public class ProductoBLL
    {
        private readonly ProductoDAL dal = new ProductoDAL();

        public List<CategoriaEntity> ObtenerCategorias()
        {
            return dal.ObtenerCategorias();
        }

        public int CrearCategoria(string nombre, string descripcion)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("Ingresa el nombre de la categoría.");
            if (dal.ExisteCategoria(nombre))
                throw new ArgumentException("Ya existe una categoría con ese nombre.");

            return dal.InsertarCategoria(nombre, descripcion);
        }

        public List<ProductoEntity> ObtenerTodos(out bool tieneColumnaStock)
        {
            return dal.ObtenerTodos(out tieneColumnaStock);
        }

        public int Guardar(ProductoEntity producto)
        {
            if (producto == null)
                throw new ArgumentNullException("producto");
            if (string.IsNullOrWhiteSpace(producto.Nombre))
                throw new ArgumentException("Ingresa el nombre del producto.");
            if (producto.IdCategoria <= 0)
                throw new ArgumentException("Selecciona una categoría.");
            if (producto.Precio < 0)
                throw new ArgumentException("El precio no puede ser negativo.");
            if (producto.Stock.HasValue && producto.Stock.Value < 0)
                throw new ArgumentException("La existencia no puede ser negativa.");

            bool esNuevo = producto.IdProducto <= 0;
            if (dal.ExisteNombre(producto.Nombre, esNuevo ? (int?)null : producto.IdProducto))
                throw new ArgumentException("Ya existe otro producto con ese nombre.");

            if (esNuevo)
                return dal.Insertar(producto);

            dal.Actualizar(producto);
            return producto.IdProducto;
        }

        public void Eliminar(int idProducto)
        {
            if (idProducto <= 0)
                throw new ArgumentException("Selecciona un producto válido.");
            dal.Eliminar(idProducto);
        }
    }
}

