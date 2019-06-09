using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GlodnyStudent.Models.Domain
{
    public class Notification
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public long RestaurantId { get; set; }
    }
}
