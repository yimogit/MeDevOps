using SqlSugar;

namespace DbDemo.DbContext
{
    public class DemoMySqlClient : SqlSugarScope, ISqlSugarClient
    {
        public DemoMySqlClient(ConnectionConfig config, Action<SqlSugarClient> configAction) : base(config, configAction)
        {
        }

    }
}
