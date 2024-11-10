using System;

namespace DBBroker.Model.Interfaces;

public interface IDataModel
{
    bool IsNotPristine(string propertyName);
}
