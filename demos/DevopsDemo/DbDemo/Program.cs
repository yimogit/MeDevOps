
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
        // �����ַ���������Ҳ���Զ�ȡ�����ļ�
        options.Configuration = App.Configuration.GetConnectionString("redis");
        // ����ǰ׺
        options.InstanceName = "test:";
    });

});
