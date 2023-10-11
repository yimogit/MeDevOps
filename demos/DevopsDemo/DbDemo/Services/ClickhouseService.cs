using Furion.DistributedIDGenerator;
using DbDemo.Models;
using SqlSugar;
using DbDemo.DbContext;

namespace DbDemo.Services
{
    [DynamicApiController]
    public class ClickhouseService
    {
        DemoClickhouseClient clickhouseClient;

        public ClickhouseService(DemoClickhouseClient clickhouseClient)
        {
            this.clickhouseClient = clickhouseClient;
        }

        /// <summary>
        /// 操作
        /// </summary>
        /// <returns></returns>
        public Ck_TestInfo Operate()
        {
            var count = clickhouseClient.Ado.SqlQuerySingle<int>("select 1");
            //插入数据
            clickhouseClient.Insertable(new Ck_TestInfo()
            {
                Id = 1,
                Name = "Test_" + ShortIDGen.NextID(),
            }).ExecuteCommand();
            var existItem = clickhouseClient.Queryable<Ck_TestInfo>().OrderByDescending(s => s.Id).First();
            return existItem;
        }

    }
}
