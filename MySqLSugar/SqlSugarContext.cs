using FanBlog.Domain.Entity;
using SqlSugar;

namespace FanBlog.Data.MySqLSugar
{
    public class SqlSugarContext
    {
        public static void InitTables(string connectionString)
        {
            SqlSugarClient client  = new SqlSugarClient(new ConnectionConfig
            {
                ConnectionString = connectionString,
                DbType = DbType.MySql,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });

            client.CodeFirst.InitTables(typeof(SystemUser));
        }
    }
}
