using MySql.Data.MySqlClient;
using System;

namespace long3x.Data.Interfaces
{
    public interface IDatabaseConnectionHelper
    {
        T Execute<T>(Func<MySqlConnection, T> action);
    }
}
