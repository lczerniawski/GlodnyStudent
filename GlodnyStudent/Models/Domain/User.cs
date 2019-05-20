﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GlodnyStudent.Models.Domain
{
    public class User
    {
        public long Id { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Name is too long")]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Email is invalid")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public RoleType Role { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public StatusType Status { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public virtual ICollection<Restaurant> Restaurants { get; set; }
    }
}
