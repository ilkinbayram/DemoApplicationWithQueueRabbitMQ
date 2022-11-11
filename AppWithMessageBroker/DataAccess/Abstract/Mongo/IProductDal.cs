using Core.DataAccess;
using Core.Entities;

namespace Mongo.DataAccess.Abstract.Mongo
{
    public interface IProductDal : IBaseRepository<Product>
    {
    }
}
