using System;

namespace DBBroker.Model.Interfaces;

public interface IDataModel
{
    bool IsPristine(string propertyName);
}
