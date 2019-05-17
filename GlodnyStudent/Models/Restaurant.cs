using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GlodnyStudent.Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Cuisine CuisineType { get; set; }
        public string Address { get; set; }
        public List<MenuItem> Menu { get; set; }
        public List<IFormFile>  Gallery { get; set; }
        public List<Review> Reviews{ get; set; }
        public int ReviewsCount { get; set; }
        public decimal HighestPrice { get; set; }
        public int OwnerId { get; set; } //Być moze do zmiany
    }
}
