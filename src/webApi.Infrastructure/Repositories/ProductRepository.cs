using Microsoft.EntityFrameworkCore;
using webApi.Data;
using webApi.Domain.Interfaces;
using webApi.Domain.Models;

namespace webApi.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DatabaseContext _context;

        public ProductRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Product?> AddAsync(Product product)
        {
            var result = await _context.Products
                .FromSqlRaw("CALL AgregarProducto({0}, {1})", product.Name, product.Price)
                .ToListAsync();
            return result.FirstOrDefault();
        }

        public async Task DeleteAsync(int id)
        {
            await _context.Database.ExecuteSqlRawAsync("CALL EliminarProducto({0})", id);
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

        public async Task<Product?> GetByNameAsync(string name)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Name == name);
        }

        public async Task<Product?> UpdateAsync(Product product)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "CALL ActualizarProducto({0}, {1}, {2})", product.Id, product.Name, product.Price);
            return await _context.Products.FindAsync(product.Id);
        }
    }
}