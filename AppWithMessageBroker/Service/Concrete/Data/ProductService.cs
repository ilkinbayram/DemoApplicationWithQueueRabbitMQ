using Core.Entities;
using Mongo.DataAccess.Abstract.Mongo;
using Service.Abstract.Data;

namespace Service.Concrete.Data
{
    public class ProductService : IProductService
    {
        private readonly IProductDal _productDal;
        public ProductService(IProductDal productDal)
        {
            _productDal = productDal;
        }
        public IEnumerable<Product> GetAll() => _productDal.GetAll();

        public Product GetById(string id) => _productDal.GetById(id);

        public void Update(Product product) => _productDal.Update(product);

        public void DeleteById(string id) => _productDal.DeleteById(id);

        public void Delete(Product product) => _productDal.Delete(product);

        public string Add(Product product) => _productDal.Add(product);
    }
}
