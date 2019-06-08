using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GeoAPI.Geometries;
using GlodnyStudent.Models;
using GlodnyStudent.Models.Domain;
using Microsoft.AspNetCore.Http;
using NetTopologySuite.Geometries;

namespace GlodnyStudent.Data
{
    public class DbInitializer
    {
        public static void Seed(ApplicationDbContext context)
        {

            if (!context.Users.Any())
            {
                context.Users.Add(new User
                {
                    Username = "Tomke",
                    Email = "Tomke@biedronke.pl",
                    Password = "AQAAAAEAACcQAAAAEAVCnvsRf0cuwEVIUpCo/9FyYUas0e87R1huT/8rKR58PR7Ib++50BEbcbnjYJnG4g==",
                    Role = RoleType.Admin,
                    Status = StatusType.Active
                });

                context.SaveChanges();
            }

            if (!context.Restaurants.Any())
            {
                context.Restaurants.AddRange(
                    new Restaurant
                    {
                        Address = null,
                        Cuisine = null,
                        Gallery = null,
                        Menu = null,
                        Name = "PizzBurg",
                        Reviews = null,
                        Score = 0,
                        ReviewsCount = 0,
                        OwnerId = context.Users.Single(u=>u.Username == "Tomke").Id
                    },
                    new Restaurant
                    {
                        Address = null,
                        Cuisine = null,
                        Gallery = null,
                        Menu = null,
                        Name = "Lodziarnia Tomka",
                        Reviews = null,
                        Score = 0,
                        ReviewsCount = 0,
                        OwnerId = context.Users.Single(u=>u.Username == "Tomke").Id
                    },
                    new Restaurant
                    {
                        Address = null,
                        Cuisine = null,
                        Gallery = null,
                        Menu = null,
                        Name = "Pizza Gonciarz",
                        Reviews = null,
                        Score = 0,
                        ReviewsCount = 0,
                        OwnerId = context.Users.Single(u=>u.Username == "Tomke").Id
                    });

                context.SaveChanges();
            }

            if (!context.RestaurantAddresses.Any())
            {
                context.RestaurantAddresses.AddRange(
                    new RestaurantAddress
                    {
                        LocalNumber = 37,
                        RestaurantId = context.Restaurants.Single(r=>r.Name == "PizzBurg").Id,
                        Street = "Sokratesa",
                        StreetNumber = "21",
                        District = "Bemowo"
                    },
                    new RestaurantAddress
                    {
                        LocalNumber = 12,
                        RestaurantId = context.Restaurants.Single(r=>r.Name == "Lodziarnia Tomka").Id,
                        Street = "Sokratesa",
                        StreetNumber = "341",
                        District = "Bemowo"
                    },
                    new RestaurantAddress
                    {
                        LocalNumber = 114,
                        RestaurantId = context.Restaurants.Single(r=>r.Name == "Pizza Gonciarz").Id,
                        Street = "Sokratesa",
                        StreetNumber = "223",
                        District = "Bemowo"
                    }
                );

                context.SaveChanges();
            }

            if (!context.Reviews.Any())
            {
                context.Reviews.AddRange(
                    new Review
                    {
                        AddTime = DateTime.Now,
                        Description = "Super Pizza",
                        RestaurantId = context.Restaurants.Single(r => r.Name == "PizzBurg").Id,
                        UserId = context.Users.Single(u => u.Username == "Tomke").Id
                    },
                    new Review
                    {
                        AddTime = DateTime.Now,
                        Description = "Dobra Pizza",
                        RestaurantId = context.Restaurants.Single(r => r.Name == "PizzBurg").Id,
                        UserId = context.Users.Single(u => u.Username == "Tomke").Id
                    },
                    new Review
                    {
                        AddTime = DateTime.Now,
                        Description = "Smaczna Pizza",
                        RestaurantId = context.Restaurants.Single(r => r.Name == "PizzBurg").Id,
                        UserId = context.Users.Single(u => u.Username == "Tomke").Id
                    },
                    new Review
                    {
                        AddTime = DateTime.Now,
                        Description = "Superr Pizza",
                        RestaurantId = context.Restaurants.Single(r => r.Name == "Pizza Gonciarz").Id,
                        UserId = context.Users.Single(u => u.Username == "Tomke").Id
                    },
                    new Review
                    {
                        AddTime = DateTime.Now,
                        Description = "Dobraa Pizza",
                        RestaurantId = context.Restaurants.Single(r => r.Name == "Pizza Gonciarz").Id,
                        UserId = context.Users.Single(u => u.Username == "Tomke").Id
                    },
                    new Review
                    {
                        AddTime = DateTime.Now,
                        Description = "Smacznaa Pizza",
                        RestaurantId = context.Restaurants.Single(r => r.Name == "Pizza Gonciarz").Id,
                        UserId = context.Users.Single(u => u.Username == "Tomke").Id
                    },
                    new Review
                    {
                        AddTime = DateTime.Now,
                        Description = "Super Lody",
                        RestaurantId = context.Restaurants.Single(r => r.Name == "Lodziarnia Tomka").Id,
                        UserId = context.Users.Single(u => u.Username == "Tomke").Id
                    },
                    new Review
                    {
                        AddTime = DateTime.Now,
                        Description = "Dobraa Lody",
                        RestaurantId = context.Restaurants.Single(r => r.Name == "Lodziarnia Tomka").Id,
                        UserId = context.Users.Single(u => u.Username == "Tomke").Id
                    },
                    new Review
                    {
                        AddTime = DateTime.Now,
                        Description = "Smacznaa Lody",
                        RestaurantId = context.Restaurants.Single(r => r.Name == "Lodziarnia Tomka").Id,
                        UserId = context.Users.Single(u => u.Username == "Tomke").Id
                    }
                );
                context.Restaurants.Single(r=>r.Name == "Lodziarnia Tomka").ReviewsCount = 3;
                context.Restaurants.Single(r=>r.Name == "Pizza Gonciarz").ReviewsCount = 3;
                context.Restaurants.Single(r=>r.Name == "PizzBurg").ReviewsCount = 3;

                context.SaveChanges();
            }

            if (!context.MenuItems.Any())
            {
                context.MenuItems.AddRange(
                        new MenuItem
                        {
                            Name = "Burger",
                            Price = 22.5M,
                            RestaurantId = context.Restaurants.Single(r=>r.Name == "PizzBurg").Id
                        },
                        new MenuItem
                        {
                            Name = "Pizza",
                            Price = 10.5M,
                            RestaurantId = context.Restaurants.Single(r=>r.Name == "PizzBurg").Id
                        },
                        new MenuItem
                        {
                            Name = "Lody Papaja",
                            Price = 21.5M,
                            RestaurantId = context.Restaurants.Single(r=>r.Name == "Lodziarnia Tomka").Id
                        },
                        new MenuItem
                        {
                            Name = "Lody  Japko",
                            Price = 10.5M,
                            RestaurantId = context.Restaurants.Single(r=>r.Name == "Lodziarnia Tomka").Id
                        },
                        new MenuItem
                        {
                            Name = "Lody Mango",
                            Price = 3.5M,
                            RestaurantId = context.Restaurants.Single(r=>r.Name == "Lodziarnia Tomka").Id
                        },
                        new MenuItem
                        {
                            Name = "Pizza1",
                            Price = 20.5M,
                            RestaurantId = context.Restaurants.Single(r=>r.Name == "Pizza Gonciarz").Id
                        },
                        new MenuItem
                        {
                            Name = "Pizza2",
                            Price = 10.5M,
                            RestaurantId = context.Restaurants.Single(r=>r.Name == "Pizza Gonciarz").Id
                        },
                        new MenuItem
                        {
                            Name = "Pizza3",
                            Price = 3.5M,
                            RestaurantId = context.Restaurants.Single(r=>r.Name == "Pizza Gonciarz").Id
                        }
                    );

                context.Restaurants.Single(r => r.Name == "Pizza Gonciarz").HighestPrice = 20.5M;
                context.Restaurants.Single(r => r.Name == "Lodziarnia Tomka").HighestPrice = 21.5M;
                context.Restaurants.Single(r => r.Name == "PizzBurg").HighestPrice = 22.5M;

                context.SaveChanges();
            }

            if (!context.Cuisines.Any())
            {
                context.Cuisines.AddRange(
                    new Cuisine
                    {
                        Name = "Amerykańska",
                        RestaurantId = context.Restaurants.Single(r=>r.Name == "PizzBurg").Id
                    },
                    new Cuisine
                    {
                        Name = "Włoska",
                        RestaurantId = context.Restaurants.Single(r=>r.Name == "Lodziarnia Tomka").Id
                    },
                    new Cuisine
                    {
                        Name = "Azjatycka",
                        RestaurantId = context.Restaurants.Single(r=>r.Name == "Pizza Gonciarz").Id
                    }
                );

                context.SaveChanges();
            }

            /*
            context.Restaurants.AddRange(
                    new Restaurant
                    {
                        Address = new RestaurantAddress{LocalNumber = 37,Location = new Point(-21,37),RestaurantId = 1,Street = "Sokratesa",StreetNumber = "21"},
                        Cuisine = new Cuisine {Name = "Amerykańska"},
                        Gallery = null,
                        Menu = new List<MenuItem>{
                            new MenuItem{Name = "Burger",Price = 22.5M,RestaurantId = 1},
                            new MenuItem{Name = "Pizza",Price = 10.5M,RestaurantId = 1},
                        },
                        Name = "PizzBurg",
                        Reviews = new List<Review>
                        {
                            new Review
                            {
                                AddTime = DateTime.Now,
                                Description = "Super 10/10",
                                RestaurantId = 1,
                                UserId = 4
                            },
                            new Review
                            {
                                AddTime = DateTime.Now,
                                Description = "Super!!!",
                                RestaurantId = 1,
                                UserId = 4
                            },
                            new Review
                            {
                                AddTime = DateTime.Now,
                                Description = "Świetny wybór!",
                                RestaurantId = 1,
                                UserId = 4
                            }
                        },
                        Score = 10,
                        ReviewsCount = 3,
                        OwnerId = 4
                    },
                    new Restaurant
                    {
                        Address = new RestaurantAddress{LocalNumber = 37,Location = new Point(-21,37),RestaurantId = 2,Street = "Sokratesa",StreetNumber = "21"},
                        Cuisine = new Cuisine {Name = "Włoska"},
                        Gallery = null,
                        Menu = new List<MenuItem>{
                            new MenuItem{Name = "Lody Papaja",Price = 21.5M,RestaurantId = 2},
                            new MenuItem{Name = "Lody  Japko",Price = 10.5M,RestaurantId = 2},
                            new MenuItem{Name = "Lody Mango",Price = 3.5M,RestaurantId = 2},
                        },
                        Name = "Lodziarnia Tomka",
                        Reviews = new List<Review>
                        {
                            new Review
                            {
                                AddTime = DateTime.Now,
                                Description = "Super 10/10",
                                RestaurantId = 2,
                                UserId = 4
                            },
                            new Review
                            {
                                AddTime = DateTime.Now,
                                Description = "Super!!!",
                                RestaurantId = 2,
                                UserId = 4
                            },
                            new Review
                            {
                                AddTime = DateTime.Now,
                                Description = "Świetny wybór!",
                                RestaurantId = 2,
                                UserId = 4
                            }
                        },
                        Score = 10,
                        ReviewsCount = 3,
                        OwnerId = 4
                    },
                    new Restaurant
                    {
                        Address = new RestaurantAddress{LocalNumber = 37,Location = new Point(-21,37),RestaurantId = 3,Street = "Sokratesa",StreetNumber = "21"},
                        Cuisine = new Cuisine {Name = "Azjatycka"},
                        Gallery = null,
                        Menu = new List<MenuItem>{
                            new MenuItem{Name = "Pizza1",Price = 20.5M,RestaurantId = 3},
                            new MenuItem{Name = "Pizza2",Price = 10.5M,RestaurantId = 3},
                            new MenuItem{Name = "Pizza3",Price = 22.5M,RestaurantId = 3},
                        },
                        Name = "Pizza Gonciarz",
                        Reviews = new List<Review>
                        {
                            new Review
                            {
                                AddTime = DateTime.Now,
                                Description = "Super 10/10",
                                RestaurantId = 3,
                                UserId = 4
                            },
                            new Review
                            {
                                AddTime = DateTime.Now,
                                Description = "Super!!!",
                                RestaurantId = 3,
                                UserId = 4
                            },
                            new Review
                            {
                                AddTime = DateTime.Now,
                                Description = "Świetny wybór!",
                                RestaurantId = 3,
                                UserId = 4
                            }
                        },
                        Score = 10,
                        ReviewsCount = 3,
                        OwnerId = 4
                    });
                    */
        }
    }
}
