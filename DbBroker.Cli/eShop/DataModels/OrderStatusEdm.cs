using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eShop.DataModels;

public class OrderStatus 
{

    private int _Id;

    [Key, Column(name: "Id")]
    public int Id
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

    private string _Status;

    [Column(name: "Status")]
    public string Status
    { 
        get
        {
            return _Status;
        } 
        set
        {
            _IsNotPristine[nameof(Status)] = true;
            _Status = value;
        }
    }


    private Dictionary<string, bool> _IsNotPristine { get; set; } = [];

    public bool IsPristine(string propertyName) => !_IsNotPristine.ContainsKey(propertyName);
}
