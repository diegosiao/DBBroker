using DBBroker.Engine;
using DBBrokerClient.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;

namespace DBBrokerClient.Persistence
{
    public class DBPerson : DBBroker<Person>
    {
        //internal static void SaveNewCar(Person person)
        //{
        //    List<DbParameter> parameters = new List<DbParameter>();
        //    parameters.Add(new SqlParameter("@IdPerson", person.Id));

        //    DbTransaction tran = GetTransaction();

        //    try
        //    {
        //        DBCar.Save(person.Car, tran);
        //        parameters.Add(new SqlParameter("@IdCar", person.Car.Id));

        //        ExecCmdSQL(
        //            cmdText: @"UPDATE Persons
        //                      SET IdCar = @IdCar,
        //                          IdPriorCar = IdCar
        //                    WHERE IdPerson = @IdPerson; ",
        //            parameters: parameters,
        //            transaction: tran);

        //        tran.Commit();
        //    }
        //    catch (Exception ex)
        //    {
        //        if (tran.Connection != null)
        //            tran.Rollback();

        //        throw ex;
        //    }
        //}
    }
}
