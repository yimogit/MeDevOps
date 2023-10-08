using DbDemo.DbContext;
using SqlSugar;

public static class SqlsugarSetup
{
    public static void AddMySqlSetup(this IServiceCollection services, IConfiguration configuration, string dbName)
    {
        //如果多个数数据库传 List<ConnectionConfig>
        var configConnection = new ConnectionConfig()
        {
            DbType = SqlSugar.DbType.MySql,
            ConnectionString = configuration.GetConnectionString(dbName),
            IsAutoCloseConnection = true,
        };

        var sqlSugar = new DemoMySqlClient(configConnection,
            db =>
            {
                //单例参数配置，所有上下文生效
                db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    Console.WriteLine(sql);//输出sql
                };
            });

        services.AddSingleton<DemoMySqlClient>(sqlSugar);//这边是SqlSugarScope用AddSingleton
    }
    public static void AddClickhouseSetup(this IServiceCollection services, IConfiguration configuration, string dbName)
    {
        //如果多个数数据库传 List<ConnectionConfig>
        var configConnection = new ConnectionConfig()
        {
            DbType = SqlSugar.DbType.ClickHouse,
            ConnectionString = configuration.GetConnectionString(dbName),
            IsAutoCloseConnection = true,
        };

        var sqlSugar = new DemoClickhouseClient(configConnection,
            db =>
            {
                //单例参数配置，所有上下文生效
                db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    Console.WriteLine(sql);//输出sql
                };
            });

        services.AddSingleton<DemoClickhouseClient>(sqlSugar);//这边是SqlSugarScope用AddSingleton
    }
}