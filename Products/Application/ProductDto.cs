using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.Application
{
    public record ProductDto(
        Guid Id,
        string Name,
        string? Description,
        double Price,
        Guid UserCreatedId,
        DateTime TimeCreated
    );

    public record CreateProductDto(
        string Name,
        string? Description,
        double Price
    );

    public record UpdateProductDto(
        string Name,
        string? Description,
        double Price
    );
}
