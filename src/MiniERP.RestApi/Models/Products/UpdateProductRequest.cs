namespace MiniERP.RestApi.Models.Products;

public record UpdateProductRequest(
    int Id,
    string Name,
    string Description,
    decimal UnitPrice,
    int CategoryId);
