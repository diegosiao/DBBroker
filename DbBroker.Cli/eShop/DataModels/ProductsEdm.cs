using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eShop.DataModels;

public class Products 
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

    private string _ProductName;

    [Column(name: "ProductName")]
    public string ProductName
    { 
        get
        {
            return _ProductName;
        } 
        set
        {
            _IsNotPristine[nameof(ProductName)] = true;
            _ProductName = value;
        }
    }


    private Dictionary<string, bool> _IsNotPristine { get; set; } = [];

    public bool IsPristine(string propertyName) => !_IsNotPristine.ContainsKey(propertyName);
}
