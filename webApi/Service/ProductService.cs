using webApi.Dto;
using webApi.Mapper;
using webApi.Model;
using webApi.Repository;
using static webApi.Exceptions.CustomExceptions;

namespace webApi.Service
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto> GetByIdAsync(int id);
        Task<ProductDto> CreateAsync(ProductDto dto);
        Task<ProductDto> UpdateAsync(int id, ProductDto dto);
        Task<bool> DeleteAsync(int id);
    }

    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly ProductMapper _mapper;

        public ProductService(IProductRepository repository, ProductMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var productos = await _repository.GetAllAsync();
            return productos.Select(_mapper.fromEntity).ToList();
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var producto = await _repository.GetByIdAsync(id);
            return _mapper.fromEntity(producto!);
        }

        public async Task<ProductDto> CreateAsync(ProductDto dto)
        {
            var existingProduct = await _repository.GetByNameAsync(dto.Name);
            if (existingProduct != null)
            {
                throw new ConflictException($"El producto con nombre '{dto.Name}' ya existe.");
            }
            Product entity = _mapper.fromDto(dto);
            await _repository.CreateAsync(entity);
            return _mapper.fromEntity(entity);
        }

        public async Task<ProductDto> UpdateAsync(int id, ProductDto dto)
        {
            Product entity = _mapper.fromDto(dto);
            entity.Id = id;
            await _repository.UpdateAsync(entity);
            return _mapper.fromEntity(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}