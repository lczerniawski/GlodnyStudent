using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using GlodnyStudent.Models;
using GlodnyStudent.Models.Domain;

namespace GlodnyStudent.ViewModels
{
    public class AddRestaurantViewModel
    {
        [Required]
        [RegularExpression(@"^[\p{Lu}\p{Ll} ]*$", ErrorMessage = "Name is invalid")]
        public string RestaurantName { get; set; }

        public string Cuisine { get; set; }

        public virtual AddressViewModel Address { get; set; }

        public string Username { get; set; }

        public bool GotOwner { get; set; }

        public double Lat { get; set; }
        public double Lng { get; set; }

    }
}
