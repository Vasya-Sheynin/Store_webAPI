using Store_webAPI.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace Store_webAPI.Controllers.Dto
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
