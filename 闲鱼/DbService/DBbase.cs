using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;

namespace 闲鱼.DbService
{
    public class DBbase
    {
        static private SqlConnection _connection;
        static SqlConnection connection { get {
                if (_connection == null)
                {
                    _connection =  new SqlConnection(Common.ConfigureHelper.Read("ConnectionStr"));
                }
                return _connection;
            } }
        public DBbase()
        {
           
        }
        public static void Execute(string Sql)
        {
            try
            {
                connection.Execute(Sql);
            }
            catch (Exception er)
            {

                throw er;
            }
        }
    }
}
