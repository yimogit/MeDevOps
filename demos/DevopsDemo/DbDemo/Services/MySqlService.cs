using Furion.DistributedIDGenerator;
using DbDemo.Models;
using SqlSugar;
using DbDemo.DbContext;

namespace DbDemo.Services
{
    [DynamicApiController]
    public class MySqlService
    {
        DemoMySqlClient mysqlClient;

        public MySqlService(DemoMySqlClient demoMySqlClient)
        {
            this.mysqlClient = demoMySqlClient;
        }

        /// <summary>
        /// 操作
        /// </summary>
        /// <returns></returns>
        public Mysql_TestInfo Operate()
        {
            mysqlClient.CodeFirst.InitTables(typeof(Mysql_TestInfo));
            mysqlClient.Insertable(new Mysql_TestInfo()
            {
                Id = IDGen.NextID(),
                Name = "Test_" + ShortIDGen.NextID(),
                Description = "TestDesc_" + ShortIDGen.NextID(),
                CreatedDate = DateTime.Now,
            }).ExecuteCommand();
            var existItem = mysqlClient.Queryable<Mysql_TestInfo>().OrderByDescending(s => s.Id).First();
            return existItem;
        }

    }
}
