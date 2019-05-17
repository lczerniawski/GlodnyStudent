using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlodnyStudent.Models
{
    public interface IRestaurantRepository
    {
        IEnumerable<Restaurant> GetRestaurantsByStreet(string streetName);
        Restaurant GetRestaurantById(int id);
        IEnumerable<Cuisine> GetAllCuisines();
        bool SaveChanges();
    }
}
