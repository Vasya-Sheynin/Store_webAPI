namespace Store_Api.Controllers.Dto
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
