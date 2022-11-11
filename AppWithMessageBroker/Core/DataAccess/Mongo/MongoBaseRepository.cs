using Core.DataAccess.Configuration.Base;
using Core.Entities.Abstract;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Core.DataAccess.Mongo
{
    public class MongoBaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : class, IEntity, new()
    {
        private readonly IMongoDatabase _database;
        private readonly string _collectionName;

        public MongoBaseRepository(IOptions<IDbConfig> dbConfig, string collectionName)
        {
            var mongoClient = new MongoClient(dbConfig.Value.CONNECTION_STRING);
            _database = mongoClient.GetDatabase(dbConfig.Value.DATABASE_NAME);
            _collectionName = collectionName;
        }

        public IEnumerable<TEntity> GetAll() => _database.GetCollection<TEntity>(_collectionName).Find(entity=>true).ToEnumerable();

        public TEntity GetById(string id) => _database.GetCollection<TEntity>(_collectionName).Find(x=>x.Id == id).FirstOrDefault();

        public string Add(TEntity entity)
        {
            _database.GetCollection<TEntity>(_collectionName).InsertOne(entity);
            return _database.GetCollection<TEntity>(_collectionName).Find(x => true).ToList().LastOrDefault().Id;
        } 


        public void DeleteById(string id) => _database.GetCollection<TEntity>(_collectionName).DeleteOne(x=>x.Id == id);

        public void Delete(TEntity entity) => _database.GetCollection<TEntity>(_collectionName).DeleteOne(x => x.Id == entity.Id);

        public void Update(TEntity entity) => _database.GetCollection<TEntity>(_collectionName).ReplaceOne(x=>x.Id == entity.Id, entity);
    }
}
