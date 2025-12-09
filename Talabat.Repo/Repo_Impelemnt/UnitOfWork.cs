using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repo.Contarct;
using Talabat.Repo.Data;

namespace Talabat.Repo.Repo_Impelemnt
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private Dictionary<string, object> _repositories;

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Dictionary<string, object>();
        }
        public IGenaricRepo<TEntity> Repository<TEntity>() where TEntity : BaseClass
        {
            var key = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(key))
            {
                var repository = new GenaricRepo<TEntity>(_dbContext);
                _repositories.Add(key, repository);
            }

            return _repositories[key] as IGenaricRepo<TEntity>;
        }


        public async Task<int> CompleteAsync() =>
            await _dbContext.SaveChangesAsync();

        public async ValueTask DisposeAsync() =>
            await _dbContext.DisposeAsync();

    }
}
