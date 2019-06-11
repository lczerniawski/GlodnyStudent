using GlodnyStudent.Models.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GlodnyStudent.Models.Repositories
{
    public interface ICuisineRepository
    {
        Task<Cuisine> Create(Cuisine cuisine);

        Task<Cuisine> FindById(long id);

        Task<Cuisine[]> FindAll();

        Task<Cuisine> Update(Cuisine cuisine);

        Task Delete(long id);
    }
}
