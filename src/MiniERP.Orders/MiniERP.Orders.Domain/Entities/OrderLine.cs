﻿namespace MiniERP.Orders.Domain.Entities;

public class OrderLine
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public int OrderId { get; set; }
}