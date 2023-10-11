
using Furion;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

Serve.Run(services =>
{
    //mysql
    services.AddMySqlSetup(App.Configuration, "mysql");

    //clickhouse
    services.AddClickhouseSetup(App.Configuration, "clickhouse"); 

    //mongo
    services.AddMongoDB(App.Configuration.GetConnectionString("mongo"));

    //redis
    services.AddStackExchangeRedisCache(options =>
    {
        // 连接字符串，这里也可以读取配置文件
        options.Configuration = App.Configuration.GetConnectionString("redis");
        // 键名前缀
        options.InstanceName = "test:";
    });

});
