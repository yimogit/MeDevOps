using SqlSugar;

namespace DbDemo.DbContext
{
    public class DemoClickhouseClient : SqlSugarScope, ISqlSugarClient
    {
        public DemoClickhouseClient(ConnectionConfig config, Action<SqlSugarClient> configAction) : base(config, configAction)
        {
        }
    }
}
