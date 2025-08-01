using webApi.Application.Dtos;
using webApi.Application.Interfaces;
using webApi.Application.Mappings;
using webApi.Domain.Interfaces;
using webApi.Domain.Models;
using static webApi.Application.Common.CustomExceptions;

namespace webApi.Application.Services
{
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
            var existingProduct = await _repository.GetByIdAsync(id);
            if (existingProduct == null)
            {
                throw new NotFoundException($"El producto con id '{id}' no existe.");
            }
            var producto = await _repository.GetByIdAsync(id);
            return _mapper.fromEntity(producto!);
        }

        public async Task<ProductDto> AddAsync(ProductDto dto)
        {
            var existingProduct = await _repository.GetByNameAsync(dto.Name);
            if (existingProduct != null)
            {
                throw new ConflictException($"El producto con nombre '{dto.Name}' ya existe.");
            }
            Product entity = _mapper.fromDto(dto);
            var savedProduct = await _repository.AddAsync(entity);
            return _mapper.fromEntity(savedProduct!);
        }

        public async Task<ProductDto> UpdateAsync(int id, ProductDto dto)
        {
            var existingProduct = await _repository.GetByIdAsync(id);
            if (existingProduct == null)
            {
                throw new NotFoundException($"El producto con id '{id}' no existe.");
            }
            Product entity = _mapper.fromDto(dto);
            entity.Id = id;
            await _repository.UpdateAsync(entity);
            return _mapper.fromEntity(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existingProduct = await _repository.GetByIdAsync(id);
            if (existingProduct == null)
            {
                throw new NotFoundException($"El producto con id '{id}' no existe.");
            }
            await _repository.DeleteAsync(id);
            return true;
        }
    }
}