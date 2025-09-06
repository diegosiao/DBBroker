namespace DbBroker.Model.Interfaces;

/// <summary>
/// Interface for data models to track changes and provide mapping information.
/// </summary>
public interface IDataModel
{
    /// <summary>
    /// Checks if the specified property has been modified since the object was created or last reset.
    /// </summary>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    bool IsNotPristine(string propertyName);

    internal DataModelMap DataModelMap { get; }
}
