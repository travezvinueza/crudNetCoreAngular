using Riok.Mapperly.Abstractions;
using webApi.Application.Dtos;
using webApi.Domain.Models;

namespace webApi.Application.Mappings
{
    [Mapper]
    public partial class ProductMapper
    {
        public partial ProductDto fromEntity(Product product);

        public partial Product fromDto(ProductDto productDto);
    }
}