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
            //创建表
            mysqlClient.CodeFirst.InitTables(typeof(Mysql_TestInfo));
            //插入数据
            mysqlClient.Insertable(new Mysql_TestInfo()
            {
                Id = IDGen.NextID(),
                Name = "Test_" + ShortIDGen.NextID(),
                Description = "TestDesc_" + ShortIDGen.NextID(),
                CreatedDate = DateTime.Now,
            }).ExecuteCommand();
            //查询数据
            var existItem = mysqlClient.Queryable<Mysql_TestInfo>().OrderByDescending(s => s.Id).First();
            return existItem;
        }

    }
}
