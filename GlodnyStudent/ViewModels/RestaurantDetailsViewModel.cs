using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlodnyStudent.Models;
using Microsoft.AspNetCore.Http;

namespace GlodnyStudent.ViewModels
{
    public class RestaurantDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public List<MenuItem> Menu { get; set; }
        public List<IFormFile> Gallery { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
