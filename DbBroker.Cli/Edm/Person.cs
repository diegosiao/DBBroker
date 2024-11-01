using System;

namespace eShop.Edm;

public class Person 
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public DateTime? Birthday { get; set; }
    public decimal? Salary { get; set; }
    public DateTime CreatedAt { get; set; }
    public required string CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? UpdatedBy { get; set; }

}
