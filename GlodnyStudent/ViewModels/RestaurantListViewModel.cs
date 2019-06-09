using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GlodnyStudent.Models
{
    public class RestaurantListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Cuisine { get; set; }
        public decimal HighestPrice { get; set; }
        public int ReviewsCount { get; set; }
        public int Score { get; set; }

    }
}
