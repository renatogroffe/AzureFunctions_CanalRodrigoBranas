using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Data.SQLite;

namespace FunctionAppProcessarAcoes.Data
{
    public static class DbConnectionFactory
    {
        public static IDbConnection Create()
        {
            var connectionString =
                Environment.GetEnvironmentVariable("BaseAcoes_Connection");
            switch (Environment.GetEnvironmentVariable("TecnologiaBD"))
            {
                case "SQLServer":
                    return new SqlConnection(connectionString);
                case "SQLite":
                    return new SQLiteConnection(connectionString);
                default:
                    throw new Exception(
                        "Tecnologia de Banco de Dados configurada incorretamente!");
            }
        }
    }
}