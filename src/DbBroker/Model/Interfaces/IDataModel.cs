namespace DbBroker.Model.Interfaces;

public interface IDataModel
{
    bool IsNotPristine(string propertyName);

    internal DataModelMap DataModelMap { get; }
}
