using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eShop.DataModels;

public class OrderProducts 
{

    private Guid _Id;

    [Key, Column(name: "Id")]
    public Guid Id
    { 
        get
        {
            return _Id;
        } 
        set
        {
            _IsNotPristine[nameof(Id)] = true;
            _Id = value;
        }
    }

    private Guid _OrderId;

    [Column(name: "OrderId")]
    public Guid OrderId
    { 
        get
        {
            return _OrderId;
        } 
        set
        {
            _IsNotPristine[nameof(OrderId)] = true;
            _OrderId = value;
        }
    }

    private Guid _ProductId;

    [Column(name: "ProductId")]
    public Guid ProductId
    { 
        get
        {
            return _ProductId;
        } 
        set
        {
            _IsNotPristine[nameof(ProductId)] = true;
            _ProductId = value;
        }
    }


    private Dictionary<string, bool> _IsNotPristine { get; set; } = [];

    public bool IsPristine(string propertyName) => !_IsNotPristine.ContainsKey(propertyName);
}
