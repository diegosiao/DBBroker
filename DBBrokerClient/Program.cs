using DBBroker.Engine;
using DBBrokerClient.ModelOracle;
using System;
using System.Reflection;

namespace DBBrokerClient
{
    class Program
    {
        static void Main()
        {
            //var parameters = new List<DbParameter>();
            //parameters.Add(new OracleParameter("pSqPessoa", OracleDbType.Varchar2) { Value = "100502304" });
            //parameters.Add(new OracleParameter("pSqContribuinte", OracleDbType.Varchar2) { Value = "1" });
            //parameters.Add(new OracleParameter("pCursor", OracleDbType.RefCursor) { Direction = ParameterDirection.Output });

            //var contador = 
            //    DBBroker<Contador>
            //    .ExecCmdSQL("UVTPKG_CONTRIBUINTE_CONSULTAS.ConsultaContador"
            //                , parameters
            //                , CommandType.StoredProcedure);


            //var empregado = new Empregado()
            //{
            //    Id = 0,
            //    Nome = "Diego",
            //    Sobrenome = "Morais",
            //};

            //DBBroker<Empregado>.Save(empregado);

            var person = new Person
            {
                Id = 21,
                Name = "Edward Snoden (Hero)",
                Birthday = new DateTime(1986, 05, 29),
                IsFriend = false,
                Salary = 50000
            };

            DBBroker<Person>.Save(person);

            //person = DBPerson.GetById(6);
            //person.Name = "Watchman Nee";
            //person.IsFriend = false;
            //DBPerson.Save(person);
        }

        /*static void Main(string[] args)
        {
            try
            {
                DateTime Step, Inicio;
                Inicio = DateTime.Now;
                Step = DateTime.Now;
                W("Início da execução: " + Inicio.ToLongDateString());

                W("");
                W("[1] CRIAÇÃO DO ARQUIVO DE CONFIGURAÇÃO CRIPTOGRAFADO (MASTER)");

                try
                {
                    Step = DateTime.Now;

                    Configuration.EncryptionPassword = new SecureString();
                    foreach (char c in "ãdfasü8dda")
                        Configuration.EncryptionPassword.AppendChar(c);

                    Configuration.EncryptConfigFile(new string[]
                    {
                        "DBBrokerClient.Model|sqlserver|Data Source=(local);Initial Catalog=master;User id=sa;Password=mila0811",
                        "DBBrokerClient.ModelMySQL|mysql|Server=localhost;Database=dbbrokertests;Uid=root;Pwd=mila0811;"
                    });

                    W("OK! Em " + (DateTime.Now - Step).TotalMilliseconds + " Milisegundos");
                }
                catch (Exception ex)
                {
                    W(@"/!\ ATENÇÃO, FALHA!\r\n" + ex.Message);
                }

                W("");
                W("[2] DBBrokerLive: CRIAÇÃO DO BANCO DE DADOS");

                try
                {
                    Step = DateTime.Now;
                    DBBrokerLive.ExecCmdSQL(@"IF EXISTS(SELECT * FROM sys.databases WHERE name = 'MyAppDB')
                                                DROP DATABASE MyAppDB;
                                              CREATE DATABASE MyAppDB; ");
                    W("OK! Em " + (DateTime.Now - Step).TotalMilliseconds + " Milisegundos");
                }
                catch (Exception ex)
                {
                    W(@"/!\ ATENÇÃO, FALHA!\r\n" + ex.Message);
                }
                
                W("");
                W("[3] CRIAÇÃO DO ARQUIVO DE CONFIGURAÇÃO CRIPTOGRAFADO (MyAppDB)");

                try
                {
                    Step = DateTime.Now;
                    Configuration.EncryptConfigFile(new string[]
                    {
                        "DBBrokerClient.Model|SQLServer|Data Source=(local);Initial Catalog=MyAppDB;User id=sa;Password=mila0811",
                        "DBBrokerClient.ModelMySQL|mysql|Server=localhost;Database=dbbrokertests;Uid=root;Pwd=mila0811;"
                    });
                    W("OK! Em " + (DateTime.Now - Step).TotalMilliseconds + " Milisegundos");
                }
                catch (Exception ex)
                {
                    W(@"/!\ ATENÇÃO, FALHA!\r\n" + ex.Message);
                }
                
                W("");
                W("[4] Configuration: RELOAD E LINHAS");

                try
                {
                    Step = DateTime.Now;
                    Configuration.Reload();

                    W("Linhas recarregadas: " + Configuration.Count);
                    
                    Print(Configuration.GetByType<Person>());
                    Print(Configuration.GetByType<MyPerson>());

                    W("OK! Em " + (DateTime.Now - Step).TotalMilliseconds + " Milisegundos");
                }
                catch (Exception ex)
                {
                    W(@"/!\ ATENÇÃO, FALHA!\r\n" + ex.Message);
                }
                
                W("");
                W("[5] SqlScriptMaker: CRIAÇÃO DOS OBJETOS");

                try
                {
                    Step = DateTime.Now;
                    SqlScriptMaker script = new SqlScriptMaker(Configuration.GetByType<Person>());
                    string sql = script.GetDatabaseScript();

                    DBBrokerLive.ExecCmdSQL(sql);
                    W("OK! Em " + (DateTime.Now - Step).TotalMilliseconds + " Milisegundos");
                }
                catch (Exception ex)
                {
                    W(@"/!\ ATENÇÃO, FALHA!\r\n" + ex.Message);
                }
                                
                W("");
                W("[6] PERSISTÊNCIAS SIMPLES: 2 Pessoas, 3 Carros");

                try
                {
                    Step = DateTime.Now;
                
                    Person person1 = new Person()
                    {
                        Name = "Watchman Nee",
                        Birth = new DateTime(1903, 11, 04),
                        Salary = 7560,
                        SomeDouble = 548.56489334,
                        SomeLong = 155448877,
                        PhotoFileName = "some.jpg"
                    };

                    DBPerson.Save(person1);
                    
                    DBCar.Save(new Car() { Model = "VOLKS Golf" });
                    DBCar.Save(new Car() { Model = "FIAT Palio" });

                    Person person2 = new Person()
                    {
                        Name = "John Wesley",
                        Birth = new DateTime(1753, 6, 28),
                        Salary = 5500,
                        SomeDouble = 548458777.56489334,
                        SomeLong = 155448877,
                        PhotoFileName = "some.jpg"
                    };

                    person2.Car = new Car() { Model = "HONDA Accord" };

                    DBPerson.Save(person2);

                    person2.ClosestFriend = person2;

                    DBPerson.Save(person2);

                    W("OK! Em " + (DateTime.Now - Step).TotalMilliseconds + " Milisegundos");
                }
                catch (Exception ex)
                {
                    W(@"/!\ ATENÇÃO, FALHA!\r\n" + ex.Message);
                }
                
                W("");
                W("[7] PERSISTÊNCIA TRANSACIONAL: Save(obj, tran)");

                try
                {
                    Step = DateTime.Now;

                    DbTransaction tran = DBPerson.GetTransaction();
                    try
                    {
                        Person person = DBPerson.GetById(1);
                        person.Car = new Car() { Model = "FORD Fiesta" };

                        DBCar.Save(person.Car, tran);

                        DBCar.Delete(1, tran);

                        DBPerson.Save(person, tran);

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (tran.Connection != null)
                            tran.Rollback();
                        throw ex;
                    }

                    W("OK! Em " + (DateTime.Now - Step).TotalMilliseconds + " Milisegundos");
                }
                catch (Exception ex)
                {
                    W(@"/!\ ATENÇÃO, FALHA!\r\n" + ex.Message);
                }

                W("");
                W("[7] PERSISTÊNCIA TRANSACIONAL: Save(obj, tran)");

                try
                {
                    Step = DateTime.Now;

                    DbTransaction tran = DBPerson.GetTransaction();
                    try
                    {
                        Person person = DBPerson.GetById(1);
                        person.Car = new Car() { Model = "FORD Fiesta" };

                        DBCar.Save(person.Car, tran);

                        DBCar.Delete(1, tran);

                        DBPerson.Save(person, tran);

                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        if (tran.Connection != null)
                            tran.Rollback();
                        throw ex;
                    }

                    W("OK! Em " + (DateTime.Now - Step).TotalMilliseconds + " Milisegundos");
                }
                catch (Exception ex)
                {
                    W(@"/!\ ATENÇÃO, FALHA!\r\n" + ex.Message);
                }

                W("");
                W("[9] PERSISTÊNCIA DE BYTE ARRAY: Save(obj, tran)");

                try
                {
                    Step = DateTime.Now;
                                
                    DbTransaction tran = DBPerson.GetTransaction();
                    try
                    {
                        Person person = DBPerson.GetById(2);

                        FileStream file1 = new FileStream(@"C:\Users\user-pc\Desktop\elephant.gif", FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        person.PhotoFileBytes = new byte[file1.Length];
                        file1.Read(person.PhotoFileBytes, 0, (int)file1.Length);

                        DBPerson.Save(person, tran);
                        tran.Commit();
                    }
                    catch (Exception)
                    {
                        if(tran.Connection != null)
                            tran.Rollback();
                        throw;
                    }
                      
                    W("OK! Em " + (DateTime.Now - Step).TotalMilliseconds + " Milisegundos");
                }
                catch (Exception ex)
                {
                    W(@"/!\ ATENÇÃO, FALHA!\r\n" + ex.Message);
                }

                W("");
                W("[9] RECUPERAÇÃO E CÓPIA DE BYTE ARRAY");

                try
                {
                    Step = DateTime.Now;

                    Person person = DBPerson.GetById(2);

                    FileStream file = new FileStream(@"C:\Users\user-pc\Desktop\elephant_copy.gif", FileMode.Create, FileAccess.ReadWrite);
                    file.Write(person.PhotoFileBytes, 0, person.PhotoFileBytes.Length);

                    W("OK! Em " + (DateTime.Now - Step).TotalMilliseconds + " Milisegundos");
                }
                catch (Exception ex)
                {
                    W(@"/!\ ATENÇÃO, FALHA!\r\n" + ex.Message);
                }

                W("");
                W("[10] DBBrokerLive: Todos os tipos primitivos");

                try
                {
                    Step = DateTime.Now;
                
                    List<DbParameter> parameters = new List<DbParameter>();
                    parameters.Add(new SqlParameter("@Datetime", new DateTime(1986, 01, 12)));
                    parameters.Add(new SqlParameter("@Inteiro", 65));
                    parameters.Add(new SqlParameter("@Bool", true));
                    parameters.Add(new SqlParameter("@Real", 15488.55477));
                    parameters.Add(new SqlParameter("@String", "Sim, teste de string"));

                    var rows = DBBrokerLive.ExecCmdSQL("SELECT @Inteiro, @Datetime, @Bool, @Real, @String; ", parameters: parameters, commandType: CommandType.Text);

                    Console.WriteLine(rows[0][0]);
                    Console.WriteLine(rows[0][1]);
                    Console.WriteLine(rows[0][2]);
                    Console.WriteLine(rows[0][3]);
                    Console.WriteLine(rows[0][4]);

                    W("OK! Em " + (DateTime.Now - Step).TotalMilliseconds + " Milisegundos");
                }
                catch (Exception ex)
                {
                    W(@"/!\ ATENÇÃO, FALHA!\r\n" + ex.Message);
                }

                W("");
                W("[11] PERSISTENCIA DE REFERENCIAS (List)");

                try
                {
                    Step = DateTime.Now;

                    Person person = DBPerson.GetById(1);
                    person.Cars = DBCar.GetAll();

                    DBPerson.Save(person);

                    W("OK! Em " + (DateTime.Now - Step).TotalMilliseconds + " Milisegundos");
                }
                catch (Exception ex)
                {
                    W(@"/!\ ATENÇÃO, FALHA!\r\n" + ex.Message);
                }

                W("");
                W("[12] PERSISTENCIA DE REFERENCIAS (List): Remoção do segundo elemento");

                try
                {
                    Step = DateTime.Now;

                    Person person = DBPerson.GetById(1);
                    person.Cars.RemoveAt(1);

                    DBPerson.Save(person);

                    W("OK! Em " + (DateTime.Now - Step).TotalMilliseconds + " Milisegundos");
                }
                catch (Exception ex)
                {
                    W(@"/!\ ATENÇÃO, FALHA!\r\n" + ex.Message);
                }

                W("");
                W("[13] CRIAÇÃO DE UMA PROCEDURE");

                try
                {
                    DBBrokerLive.ExecCmdSQL(@"  CREATE PROCEDURE usp_Persons
                                                AS
                                                BEGIN
	                                                SELECT * FROM Persons;
                                                END");

                    W("OK! Em " + (DateTime.Now - Step).TotalMilliseconds + " Milisegundos");
                }
                catch (Exception ex)
                {
                    W(@"/!\ ATENÇÃO, FALHA!\r\n" + ex.Message);
                }      

                W("");
                W("[14] DADOS DE UMA PROCEDURE");

                try
                {
                    var result = DBBrokerLive.ExecCmdSQL(@"usp_Persons");

                    foreach (var row in result)
                    {
                        W("====================================");
                        foreach (var column in row.ColumnNames)
                            W(column + ": " + row[column]);
                    }

                    W("OK! Em " + (DateTime.Now - Step).TotalMilliseconds + " Milisegundos");
                }
                catch (Exception ex)
                {
                    W(@"/!\ ATENÇÃO, FALHA!\r\n" + ex.Message);
                }
                                                     
                W("");
                W("");
                W("TEMPO TOTAL DA EXECUÇÃO: " + (DateTime.Now - Inicio).TotalMilliseconds + " Milisegundos");
                W("");
                W("Pressione enter para finalizar...");
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            Console.ReadLine();
        }
        */
        static void W(object obj)
        {
            if (obj == null)
                Console.WriteLine("-null-");
            else
                Console.WriteLine(obj);
        }

        static void Print(object obj)
        {
            W("===================================");

            if (obj == null)
            {
                W("NULL");
            }
            else
            {
                W("[" + obj.GetType().Name + "]");

                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                    W(prop.Name + ": " + prop.GetValue(obj, null));
            }

            W("===================================");
            return;
        }
    }
}
