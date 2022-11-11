using Core.Entities.Abstract;

namespace Core.DataAccess
{
    public interface IBaseRepository<T> where T : class, IEntity, new()
    {
        IEnumerable<T> GetAll();
        T GetById(string id);
        string Add(T entity);
        void DeleteById(string id);
        void Delete(T entity);
        void Update(T entity);
    }
}
