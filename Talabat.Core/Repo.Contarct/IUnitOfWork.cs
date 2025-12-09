using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Repo.Contarct
{ 
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenaricRepo<TEntity> Repository<TEntity>() where TEntity : BaseClass;

        Task<int> CompleteAsync();
    }
}
