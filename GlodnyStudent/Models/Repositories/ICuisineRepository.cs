using GlodnyStudent.Models.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GlodnyStudent.Models.Repositories
{
    interface ICuisineRepository
    {
        Task<Cuisine> Create(Cuisine cuisine);

        Task<Cuisine> FindByName(string name);

        Task<IEnumerable<Cuisine>> FindAll();

        Task<Cuisine> Update(Cuisine cuisine);

        Task Delete(string name);
    }
}
