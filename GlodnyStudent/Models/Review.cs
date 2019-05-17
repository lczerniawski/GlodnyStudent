using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace GlodnyStudent.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public string Description { get; set; }
        public DateTime AddTime { get; set; }
        public int ReviewerId { get; set; } //Być moze do zmiany
    }
}
