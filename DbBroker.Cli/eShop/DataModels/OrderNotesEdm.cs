using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eShop.DataModels;

public class OrderNotes 
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

    private string _NoteContent;

    [Column(name: "NoteContent")]
    public string NoteContent
    { 
        get
        {
            return _NoteContent;
        } 
        set
        {
            _IsNotPristine[nameof(NoteContent)] = true;
            _NoteContent = value;
        }
    }


    private Dictionary<string, bool> _IsNotPristine { get; set; } = [];

    public bool IsPristine(string propertyName) => !_IsNotPristine.ContainsKey(propertyName);
}
