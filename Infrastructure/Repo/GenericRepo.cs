using Infrastructure;
using Infrastructure.Repo.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace Infrastructure.Repo
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        private readonly AppDbContext _db;
        protected readonly DbSet<T> _dbSet;

        public GenericRepo(AppDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
        }

        // ===================== CRUD =====================

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }


        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _db.SaveChangesAsync();
        }

        // ===================== خيارات إضافية =====================
        // مثال: البحث باستخدام شرط
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }
    }
}
