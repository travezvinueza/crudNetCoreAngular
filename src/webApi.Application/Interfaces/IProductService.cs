using webApi.Application.Dtos;

namespace webApi.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto> GetByIdAsync(int id);
        Task<ProductDto> AddAsync(ProductDto dto);
        Task<ProductDto> UpdateAsync(int id, ProductDto dto);
        Task<bool> DeleteAsync(int id);
    }
}