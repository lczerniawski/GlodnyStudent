using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GeoAPI.Geometries;
using GlodnyStudent.Models;
using GlodnyStudent.Models.Domain;
using Microsoft.AspNetCore.Http;
using NetTopologySuite.Geometries;

namespace GlodnyStudent.Data
{
    public class DbInitializer
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (!context.Users.Any())
            {
                context.Users.Add(new User
                {
                    Username = "Admin",
                    Email = "admin@glodnystudent.pl",
                    Password = "AQAAAAEAACcQAAAAEHtNEXNHq5ryGBizjkVg+FKywVMJ8EZW9pEf8FFQ8KocechAL+bjpLs4kvcd+tBMCA==",
                    Role = RoleType.Admin,
                    Status = StatusType.Active
                });

                context.SaveChanges();
            }
        }
    }
}
