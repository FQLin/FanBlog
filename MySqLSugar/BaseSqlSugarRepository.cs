using Microsoft.Extensions.Options;
using SqlSugar;

namespace FanBlog.Data.MySqLSugar
{
    public abstract class BaseSqlSugarRepository
    {
        protected readonly SqlSugarClient _db;

        protected BaseSqlSugarRepository(SqlSugarClient client)
        {
            this._db = client;
        }
    }
}
