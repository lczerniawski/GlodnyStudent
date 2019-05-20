using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlodnyStudent.Models.Domain;

namespace GlodnyStudent.Models.Repositories
{
    public interface IRestaurantRepository
    {
        IEnumerable<Restaurant> GetRestaurantsByStreet(string streetName);
        Restaurant GetRestaurantById(int id);
        IEnumerable<Cuisine> GetAllCuisines();
        Review AddReview(Restaurant restaurant, Review review);
        bool SaveChanges();
    }
}
