using System;

namespace eShop.Edm;

public class Car 
{
    public Guid Id { get; set; }
    public Guid? PersonId { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string Plates { get; set; }

}
