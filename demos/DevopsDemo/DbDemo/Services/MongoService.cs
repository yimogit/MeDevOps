using Furion.DistributedIDGenerator;
using DbDemo.Models;
using SqlSugar;
using DbDemo.DbContext;
using MongoDB.Driver;
using MongoDB.Bson;

namespace DbDemo.Services
{
    [DynamicApiController]
    public class MongoService
    {
        private readonly IMongoDBRepository _mongoRepository;

        public MongoService(IMongoDBRepository mongoRepository)
        {
            _mongoRepository = mongoRepository;
        }

        /// <summary>
        /// 操作
        /// </summary>
        /// <returns></returns>
        public async Task Operate()
        {
            //test数据库需要存在并有权限
            var database = _mongoRepository.Context.GetDatabase("test");
            var collection = database.GetCollection<BsonDocument>("bar");

            await collection.InsertOneAsync(new BsonDocument("Name", "Jack"));

            var list = await collection.Find(new BsonDocument("Name", "Jack"))
                .ToListAsync();

            foreach (var document in list)
            {
                Console.WriteLine(document["Name"]);
            }
        }

    }
}
