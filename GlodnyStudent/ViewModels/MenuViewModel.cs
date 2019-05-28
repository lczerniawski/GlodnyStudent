using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GlodnyStudent.ViewModels
{
    public class MenuViewModel
    {
        public long Id { get; set; }

        [Required]
        [RegularExpression(@"^[\p{Lu}\p{Ll} ]*$", ErrorMessage = "Name is invalid")]
        public string Name { get; set; }

        [Column(TypeName = "decimal(13,2)")]
        public decimal Price { get; set; }
    }
}
