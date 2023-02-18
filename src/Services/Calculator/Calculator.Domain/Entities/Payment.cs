using Shared.Domain.Base;

namespace Calculator.Domain.Entities;

public class Payment : Entity
{
    public int PersonToAccountedForId { get; set; }
    public int PieceworkProductId { get; set; }
    public decimal Quantity { get; set; }  
}