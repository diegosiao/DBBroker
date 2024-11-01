using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eShop.DataModels;

public class Customers 
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

    private string _Name;

    [Column(name: "Name")]
    public string Name
    { 
        get
        {
            return _Name;
        } 
        set
        {
            _IsNotPristine[nameof(Name)] = true;
            _Name = value;
        }
    }

    private DateTime? _Birthday;

    [Column(name: "Birthday")]
    public DateTime? Birthday
    { 
        get
        {
            return _Birthday;
        } 
        set
        {
            _IsNotPristine[nameof(Birthday)] = true;
            _Birthday = value;
        }
    }

    private int? _OrdersTotal;

    [Column(name: "OrdersTotal")]
    public int? OrdersTotal
    { 
        get
        {
            return _OrdersTotal;
        } 
        set
        {
            _IsNotPristine[nameof(OrdersTotal)] = true;
            _OrdersTotal = value;
        }
    }

    private DateTime _CreatedAt;

    [Column(name: "CreatedAt")]
    public DateTime CreatedAt
    { 
        get
        {
            return _CreatedAt;
        } 
        set
        {
            _IsNotPristine[nameof(CreatedAt)] = true;
            _CreatedAt = value;
        }
    }

    private string _CreatedBy;

    [Column(name: "CreatedBy")]
    public string CreatedBy
    { 
        get
        {
            return _CreatedBy;
        } 
        set
        {
            _IsNotPristine[nameof(CreatedBy)] = true;
            _CreatedBy = value;
        }
    }

    private DateTime? _ModifiedAt;

    [Column(name: "ModifiedAt")]
    public DateTime? ModifiedAt
    { 
        get
        {
            return _ModifiedAt;
        } 
        set
        {
            _IsNotPristine[nameof(ModifiedAt)] = true;
            _ModifiedAt = value;
        }
    }

    private string? _ModifiedBy;

    [Column(name: "ModifiedBy")]
    public string? ModifiedBy
    { 
        get
        {
            return _ModifiedBy;
        } 
        set
        {
            _IsNotPristine[nameof(ModifiedBy)] = true;
            _ModifiedBy = value;
        }
    }


    private Dictionary<string, bool> _IsNotPristine { get; set; } = [];

    public bool IsPristine(string propertyName) => !_IsNotPristine.ContainsKey(propertyName);
}
