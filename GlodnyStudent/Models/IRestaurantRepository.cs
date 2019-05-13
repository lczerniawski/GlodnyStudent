using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlodnyStudent.Models
{
    public interface IRestaurantRepository
    {
        IEnumerable<RestaurantViewModel> GetRestaurantsByStreet(string streetName);
    }
}
