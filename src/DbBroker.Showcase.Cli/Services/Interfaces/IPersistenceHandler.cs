using System;

namespace DbBroker.Showcase.Cli.Services.Interfaces;

public interface IPersistenceHandler
{
    Task CreateNewOrder<TDataModel>(TDataModel order);

    Task UpdateCustomer<TDataModel>(TDataModel customer);

    Task<TDataModel> DeleteCustomer<TDataModel>(TDataModel customer);

    
}
