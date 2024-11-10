using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.Application
{
    public record ProductDto(
        [Required] Guid Id,
        [Required] string Name,
        string? Description,
        [Required] double Price,
        [Required] Guid UserCreatedId,
        [Required] DateTime TimeCreated
    );

    public record CreateProductDto(
        [Required] string Name,
        string? Description,
        [Required] double Price
    );

    public record UpdateProductDto(
        [Required] string Name,
        string? Description,
        [Required] double Price
    );
}
