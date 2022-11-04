using Authentication.Data.Context;
using Authentication.Shared.Base;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Authentication.Data.Repositories.Base
{
    public class RepositoryBase<TEntity, TId> : IRepositoryBase<TEntity, TId> where TEntity : EntityBase where TId : struct
    {
        private readonly AppDbContext _context;

        public RepositoryBase(AppDbContext context)
        {
            _context = context;
        }

        public IList<TEntity> GetAll()
        {
            var entidade = _context.Set<TEntity>().ToList();
            return entidade;
        }

        public TEntity GetById(int id)
        {
            var entidade = _context.Set<TEntity>().FirstOrDefault(item => item.Id == id);
            return entidade;
        }

        public virtual TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            var entidade = _context.Set<TEntity>().FirstOrDefault(predicate);
            return entidade;
        }

        public TEntity Add(TEntity entidade)
        {
            _context.Set<TEntity>().Add(entidade);
            _context.SaveChanges();
            return entidade;
        }

        public IEnumerable<TEntity> AddList(IEnumerable<TEntity> entidades)
        {
            _context.Set<TEntity>().AddRange(entidades);
            _context.SaveChanges();
            return entidades;
        }

        public TEntity Update(TEntity entidade)
        {
            _context.Entry(entidade).State = EntityState.Modified;
            _context.SaveChanges();
            return entidade;
        }

        public void Delete(TEntity entidade)
        {
            _context.Set<TEntity>().Remove(entidade);
            _context.SaveChanges();
        }

        public bool Exist(Func<TEntity, bool> where) => _context.Set<TEntity>().Any(where);

        public virtual void Dispose() => _context.Dispose();
    }

}
