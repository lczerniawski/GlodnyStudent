using System.Collections.Generic;
using System.Threading.Tasks;

namespace GlodnyStudent.Models.Repositories
{
    public interface ICrudRepository<T>
    {
        Task<T> Create(T obj);

        Task<T> FindById(long id);

        Task<IEnumerable<T>> FindAll();

        Task<T> Update(T obj);

        Task Delete(long id);
    }
}
