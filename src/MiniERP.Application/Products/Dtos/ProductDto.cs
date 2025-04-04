﻿namespace MiniERP.Application.Products.Dtos;

public class ProductDto
{
    public int? Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal UnitPrice { get; set; }
    public CategoryDto Category { get; set; }
}