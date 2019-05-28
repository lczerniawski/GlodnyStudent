using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlodnyStudent.Models;
using GlodnyStudent.Models.Domain;
using Microsoft.AspNetCore.Http;

namespace GlodnyStudent.ViewModels
{
    public class RestaurantDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AddressViewModel Address { get; set; }
        public List<MenuViewModel> Menu { get; set; }
        public List<IFormFile> Gallery { get; set; }
        public List<ReviewViewModel> Reviews { get; set; }
        public int Score { get; set; }
    }
}
