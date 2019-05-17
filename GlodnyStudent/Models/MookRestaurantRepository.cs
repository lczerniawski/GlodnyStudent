using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Remotion.Linq.Clauses;

namespace GlodnyStudent.Models
{
    public class MookRestaurantRepository : IRestaurantRepository
    {
        private readonly List<Cuisine> _cuisines;
        private readonly List<Restaurant> _restaurants;

        public MookRestaurantRepository()
        {
            _cuisines = new List<Cuisine>
            {
                new Cuisine {Name = "Amerykańska"},
                new Cuisine {Name = "Azjatycka"},
                new Cuisine {Name = "Włoska"},
                new Cuisine {Name = "Polska"},
            };

            _restaurants = new List<Restaurant>
            {
                new Restaurant()
                {
                    Id = 1,
                    Name = "Tomke Biedronke",
                    CuisineType = _cuisines.First(r => r.Name == "Amerykańska"),
                    Address = "Sokratesa",
                    Menu = new List<MenuItem>()
                    {
                        new MenuItem
                        {
                            Id = 1,
                            Name = "Burger",
                            Price = 22.5M,
                        },
                        new MenuItem
                        {
                            Id = 2,
                            Name = "Surufka",
                            Price = 10.5M,
                        },
                        new MenuItem
                        {
                            Id = 3,
                            Name = "Mienso",
                            Price = 5.5M,
                        },
                    },
                    Gallery = new List<IFormFile>(),
                    Reviews = new List<Review>()
                    {
                        new Review
                        {
                            Id = 1,
                            AddTime = DateTime.Now,
                            Description = "Super 20/30",
                            ReviewerId = 1,
                        },
                        new Review
                        {
                            Id = 2,
                            AddTime = DateTime.Now,
                            Description = "Kozak",
                            ReviewerId = 1,
                        },
                        new Review
                        {
                            Id = 3,
                            AddTime = DateTime.Now,
                            Description = "Wspaniale",
                            ReviewerId = 1,
                        }
                    },
                    ReviewsCount = 3,
                    Score = 10,
                    HighestPrice = 22.5M,
                    OwnerId = 0
                },
                new Restaurant()
                {
                    Id = 2,
                    Name = "Pizza Tomka",
                    CuisineType = _cuisines.First(r => r.Name == "Włoska"),
                    Address = "Sokratesa",
                    Menu = new List<MenuItem>()
                    {
                        new MenuItem
                        {
                            Id = 1,
                            Name = "Pizza Hawajska",
                            Price = 22.5M,
                        },
                        new MenuItem
                        {
                            Id = 2,
                            Name = "Pizza Kebab",
                            Price = 10.5M,
                        },
                        new MenuItem
                        {
                            Id = 3,
                            Name = "Pizza Mienso",
                            Price = 5.5M,
                        },
                    },
                    Gallery = null,
                    Reviews = new List<Review>()
                    {
                        new Review
                        {
                            Id = 1,
                            AddTime = DateTime.Now,
                            Description = "Super 20/30",
                            ReviewerId = 1,
                        },
                        new Review
                        {
                            Id = 2,
                            AddTime = DateTime.Now,
                            Description = "Kozak",
                            ReviewerId = 1,
                        },
                        new Review
                        {
                            Id = 3,
                            AddTime = DateTime.Now,
                            Description = "Wspaniale",
                            ReviewerId = 1,
                        }
                    },
                    ReviewsCount = 3,
                    HighestPrice = 22.5M,
                    Score = 10,
                    OwnerId = 0
                },
                new Restaurant()
                {
                    Id = 3,
                    Name = "Lodziarnia Tomka",
                    CuisineType = _cuisines.First(r => r.Name == "Włoska"),
                    Address = "Sokratesa",
                    Menu = new List<MenuItem>()
                    {
                        new MenuItem
                        {
                            Id = 1,
                            Name = "Lody Kremowkowe",
                            Price = 22.5M,
                        },
                        new MenuItem
                        {
                            Id = 2,
                            Name = "Lody Papaja",
                            Price = 10.5M,
                        },
                        new MenuItem
                        {
                            Id = 3,
                            Name = "Lody Mango",
                            Price = 5.5M,
                        },
                    },
                    Gallery = null,
                    Reviews = new List<Review>()
                    {
                        new Review
                        {
                            Id = 1,
                            AddTime = DateTime.Now,
                            Description = "Super 20/30",
                            ReviewerId = 1,
                        },
                        new Review
                        {
                            Id = 2,
                            AddTime = DateTime.Now,
                            Description = "Kozak",
                            ReviewerId = 1,
                        },
                        new Review
                        {
                            Id = 3,
                            AddTime = DateTime.Now,
                            Description = "Wspaniale",
                            ReviewerId = 1,
                        }
                    },
                    ReviewsCount = 3,
                    HighestPrice = 22.5M,
                    Score = 10,
                    OwnerId = 0
                }
            };
        }


        public IEnumerable<Restaurant> GetRestaurantsByStreet(string streetName)
        {
            var restaurants = from r in _restaurants
                where r.Address == streetName
                select r;

            return restaurants;
        }

        public Restaurant GetRestaurantById(int id)
        {
            return _restaurants.FirstOrDefault(r => r.Id == id); ;
        }

        public IEnumerable<Cuisine> GetAllCuisines()
        {
            return _cuisines;
        }

        public bool SaveChanges()
        {
            return true;
        }
    }
}