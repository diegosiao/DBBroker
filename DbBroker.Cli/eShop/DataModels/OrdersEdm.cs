using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eShop.DataModels;

public class Orders 
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

    private Guid _CustomerId;

    [Column(name: "CustomerId")]
    public Guid CustomerId
    { 
        get
        {
            return _CustomerId;
        } 
        set
        {
            _IsNotPristine[nameof(CustomerId)] = true;
            _CustomerId = value;
        }
    }

    private int? _StatusId;

    [Column(name: "StatusId")]
    public int? StatusId
    { 
        get
        {
            return _StatusId;
        } 
        set
        {
            _IsNotPristine[nameof(StatusId)] = true;
            _StatusId = value;
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
