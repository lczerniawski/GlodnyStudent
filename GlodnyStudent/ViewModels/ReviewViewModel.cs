using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using GlodnyStudent.Models.Domain;

namespace GlodnyStudent.ViewModels
{
    public class ReviewViewModel
    {
        public long Id { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        public DateTime AddTime { get; set; }
        
        public string UserName { get; set; }

    }
}
