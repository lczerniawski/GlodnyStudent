using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GeoAPI.Geometries;

namespace GlodnyStudent.ViewModels
{
    public class AddressViewModel
    {
        [Required(ErrorMessage = "Street is required")]
        [RegularExpression(@"^[\p{Lu}\p{Ll}0-9 ]*$", ErrorMessage = "Street is invalid")]
        public string StreetName { get; set; }

        [Required(ErrorMessage = "Street number is required")]
        [MaxLength(7, ErrorMessage = "Street number is too long")]
        [RegularExpression(@"^[\p{Lu}\p{Ll}0-9 ]*$", ErrorMessage = "Street number is invalid")]
        public string StreetNumber { get; set; }
                
        public int LocalNumber { get; set; }

        public string District { get; set; }

        public double LocationX { get; set; }
        public double LocationY { get; set; }

    }
}
