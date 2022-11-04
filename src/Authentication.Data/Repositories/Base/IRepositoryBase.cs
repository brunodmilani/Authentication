using System.Linq.Expressions;

namespace Authentication.Data.Repositories.Base
{
    public interface IRepositoryBase<TEntity, in TId> where TEntity : class where TId : struct
    {
        IList<TEntity> GetAll();
        TEntity GetById(int id);
        TEntity Find(Expression<Func<TEntity, bool>> predicate);
        TEntity Add(TEntity entity);
        IEnumerable<TEntity> AddList(IEnumerable<TEntity> entities);
        void Delete(TEntity entity);
        bool Exist(Func<TEntity, bool> where);
        TEntity Update(TEntity entity);
        void Dispose();
    }

}
