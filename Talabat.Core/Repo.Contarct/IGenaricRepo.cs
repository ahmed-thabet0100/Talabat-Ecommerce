using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repo.Contarct
{
    public interface IGenaricRepo<T> where T : BaseClass
    {
        Task<T?> GetAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T Entity);
        void Update(T Entity);
        void Delete(T Entity);

        #region specification
        // ✅ Advanced methods using Specification Pattern
        Task<T?> GetEntityWithSpecAsync(ISpecification<T> spec);
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec);

        // ✅ Count method (مفيدة في pagination)
        Task<int> CountAsync(ISpecification<T> spec);

        #endregion 
    }
}
