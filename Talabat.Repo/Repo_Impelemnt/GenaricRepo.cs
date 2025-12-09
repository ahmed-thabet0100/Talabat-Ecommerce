using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repo.Contarct;
using Talabat.Core.Specifications;
using Talabat.Repo.Data;
using Talabat.Repo.Helpers;

namespace Talabat.Repo
{
    public class GenaricRepo<T> : IGenaricRepo<T> where T : BaseClass
    {
        private readonly AppDbContext _dbContext;

        public GenaricRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T?> GetAsync(int id)
        {
            //if (typeof(T) == typeof(Product))
            //    return await _dbContext.Set<Product>().Where(x => x.Id == id).Include(p => p.Brand).Include(p => p.Category).FirstOrDefaultAsync() as T;
            return await _dbContext.Set<T>().FindAsync(id);
        }


        #region specification
        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }
        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);
        }
        public async Task<T?> GetEntityWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }
        public async Task<int> CountAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        #endregion
        public async Task AddAsync(T Entity)
        {
            await _dbContext.AddAsync(Entity);
            _dbContext.SaveChanges();
        }

        public async void Update(T Entity)
        {
             _dbContext.Update(Entity);
            _dbContext.SaveChanges();
        }

        public void Delete(T Entity)
        {
            _dbContext.Remove(Entity);
            _dbContext.SaveChanges();
        }

    }
}
