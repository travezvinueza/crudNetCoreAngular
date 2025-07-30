using Riok.Mapperly.Abstractions;
using webApi.Dto;
using webApi.Model;

namespace webApi.Mapper
{
    [Mapper]
    public partial class ProductMapper
    {
        public partial ProductDto fromEntity(Product product);

        public partial Product fromDto(ProductDto productDto);
    
    }
}