using MiniERP.Application.Exceptions;

public class ProductNotFoundException : NotFoundException
{
    public ProductNotFoundException(int productId)
        : base($"Product with ID {productId} was not found.") { }
}