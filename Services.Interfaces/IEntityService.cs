using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IEntityService<T> where T : Entity
    {
        Task<T> CreateAsync(T entity);
        Task<IEnumerable<T>> ReadAsync();
        Task<T?> ReadAsync(int id);
        Task UpdateAsync(int id, T entity);
        Task DeleteAsync(int id);
    }
}
