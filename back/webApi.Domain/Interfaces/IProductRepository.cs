using webApi.Domain.Models;

namespace webApi.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync(string? nombre = "", int page = 1, int size = 10);
        Task<Product?> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task<Product?> AddAsync(Product product);
        Task<Product?> UpdateAsync(Product product);
        Task<Product?> GetByNameAsync(string name);
    }
}