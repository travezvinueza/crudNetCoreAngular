using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace webApi.Dto
{
    [JsonUnmappedMemberHandling(JsonUnmappedMemberHandling.Disallow)]
    public class ProductDto
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio es obligatorio.")]
        public decimal Price { get; set; }

    }
}