using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Remotion.Linq.Clauses;

namespace GlodnyStudent.Models
{
    public class FakeRestuarantRepository : IRestaurantRepository
    {
        private IEnumerable<RestaurantViewModel> _restaurants = new List<RestaurantViewModel>
        {
            new RestaurantViewModel()
            {
                Address = "Sokratesa",
                Cuisine = "Amerykanska",
                Distance = 2,
                HighestPrice = 25,
                Id = 1,
                Name = "Tomke Biedronke",
                ReviewsCount = 39
            },
            new RestaurantViewModel()
            {
                Address = "Sokratesa",
                Cuisine = "Wloska",
                Distance = 3,
                HighestPrice = 30,
                Id = 2,
                Name = "Biedronke",
                ReviewsCount = 9
            },
            new RestaurantViewModel()
            {
                Address = "Sokratesa",
                Cuisine = "Japonska",
                Distance = 10,
                HighestPrice = 40,
                Id = 3,
                Name = "Biedronke suszi",
                ReviewsCount = 2137
            }
        };
        public IEnumerable<RestaurantViewModel> GetRestaurantsByStreet(string streetName)
        {
            var resturants = from r in _restaurants
                where r.Address == streetName
                select r;

            return resturants;
        }
    }
}
