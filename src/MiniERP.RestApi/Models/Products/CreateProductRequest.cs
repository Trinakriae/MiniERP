namespace MiniERP.RestApi.Models.Products;

public record CreateProductRequest(
    string Name,
    string Description,
    decimal UnitPrice,
    int CategoryId);
