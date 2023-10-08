
using Furion;

Serve.Run(services =>
{
    services.AddMySqlSetup(App.Configuration, "mysql");
    services.AddClickhouseSetup(App.Configuration, "clickhouse");
});
