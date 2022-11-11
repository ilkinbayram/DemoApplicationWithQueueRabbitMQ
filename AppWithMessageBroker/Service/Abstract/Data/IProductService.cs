using Core.Entities;

namespace Service.Abstract.Data
{
    public interface IProductService
    {
        IEnumerable<Product> GetAll();
        Product GetById(string id);
        string Add(Product entity);
        void DeleteById(string id);
        void Delete(Product entity);
        void Update(Product entity);
    }
}
