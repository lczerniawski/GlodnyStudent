using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GlodnyStudent.Models.Domain;

namespace GlodnyStudent.ViewModels
{
    public class AddReviewViewModel
    {
        public string Description { get; set; }

        public DateTime AddTime { get; set; }
        
        public long UserId { get; set; }

        public long RestaurantId { get; set; }

    }
}
