﻿namespace Calculator.Infrastructure.DataAccessObjects.AccountDataAccessObjects;

public class ProductItemDao
{
    public Guid Id { get; init; }
    public Guid AccountId { get; init; }
    public Guid PieceworkProductId { get; init; }
    public decimal CurrentPrice { get; init; }
    public decimal Quantity { get; init; }
    public bool IsConsidered { get; init; }
    public DateTime Date { get; init; }
}