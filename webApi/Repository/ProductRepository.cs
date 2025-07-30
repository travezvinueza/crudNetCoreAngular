using Microsoft.EntityFrameworkCore;
using webApi.Data;
using webApi.Model;

namespace webApi.Repository
{
    public interface IProductRepository
    {
         Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task CreateAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<Product?> GetByNameAsync(string name);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly DatabaseContext _context;

        public ProductRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products
                .FromSqlRaw("CALL ListarProductos()")
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task CreateAsync(Product product)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "CALL AgregarProducto({0}, {1})", product.Name, product.Price);
        }

        public async Task UpdateAsync(Product product)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "CALL ActualizarProducto({0}, {1}, {2})", product.Id, product.Name, product.Price);
        }

        public async Task DeleteAsync(int id)
        {
            await _context.Database.ExecuteSqlRawAsync("CALL EliminarProducto({0})", id);
        }

        public async Task<Product?> GetByNameAsync(string name)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Name == name);
        }
    }
}