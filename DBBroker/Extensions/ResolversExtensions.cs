using System;
using System.Data.Common;
using DBBroker.Model;

namespace DBBroker.Extensions;

public static class ResolversExtensions
{
    public static DbConnection GetConnection<TDataModel>(this DataModelBase dataModelBase)
    {
        return null;
    }
}
