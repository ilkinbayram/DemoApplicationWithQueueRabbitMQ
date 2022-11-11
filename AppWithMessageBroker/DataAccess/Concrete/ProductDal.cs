using Core.DataAccess.Configuration;
using Core.DataAccess.Mongo;
using Core.Entities;
using Microsoft.Extensions.Options;
using Mongo.DataAccess.Abstract.Mongo;

namespace Mongo.DataAccess.Concrete
{
    public class ProductDal : MongoBaseRepository<Product>, IProductDal
    {
        public ProductDal(IOptions<DbConfig> dbConfig) : base(dbConfig, "Products")
        {
        }
    }
}
